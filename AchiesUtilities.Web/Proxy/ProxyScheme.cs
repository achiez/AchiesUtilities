using AchiesUtilities.Web.Proxy.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using AchiesUtilities.Extensions;

namespace AchiesUtilities.Web.Proxy;

public class ProxyScheme
{
    public static class SchemeGroups
    {
        public static readonly string Protocol = "PROTOCOL";
        public static readonly string Host = "HOST";
        public static readonly string Port = "PORT";
        public static readonly string Username = "USER";
        public static readonly string Password = "PASSWORD";
    }

    public Regex Regex { get; }
    public bool ProtocolRequired { get; }
    public ProxyProtocol? DefaultProtocol { get; }
    public ProxyPatternProtocol AllowedProtocols { get; }
    public ProxyPatternHostFormat AllowedFormats { get; }

    public PatternRequirement UsernameRequirement { get; }
    public PatternRequirement PasswordRequirement { get; }


    public ProxyScheme(Regex regex, bool protocolRequired, ProxyProtocol? defaultProtocol, ProxyPatternProtocol allowedProtocols, ProxyPatternHostFormat allowedFormats, PatternRequirement usernameRequirement, PatternRequirement passwordRequirement)
    {
        Regex = regex;
        ProtocolRequired = protocolRequired;
        DefaultProtocol = defaultProtocol;
        AllowedProtocols = allowedProtocols;
        AllowedFormats = allowedFormats;
        UsernameRequirement = usernameRequirement;
        PasswordRequirement = passwordRequirement;
        ValidateRegexAndState(regex);
    }


    private void ValidateRegexAndState(Regex regex)
    {
        var groups = regex.GetGroupNames();
        if (groups.All(g => g.Equals(SchemeGroups.Host) == false)
                            ||
            groups.All(g => g.Equals(SchemeGroups.Port) == false))
        {
            throw new InvalidOperationException(
                $"Invalid Proxy Regex. It must contain {SchemeGroups.Host} and {SchemeGroups.Port} groups");
        }

        if (ProtocolRequired)
        {
            ValidateOrThrow(nameof(ProtocolRequired), SchemeGroups.Protocol);
        }
        else if (DefaultProtocol == null)
        {
            throw new InvalidOperationException(
                $"Invalid ProxyScheme. {nameof(ProtocolRequired)} is true but no {nameof(DefaultProtocol)} is set");
        }

        if (UsernameRequirement == PatternRequirement.Required)
        {
            ValidateOrThrow(nameof(UsernameRequirement), SchemeGroups.Username);
        }

        if (UsernameRequirement == PatternRequirement.Required && PasswordRequirement == PatternRequirement.Required)
        {
            ValidateOrThrow(nameof(PasswordRequirement), SchemeGroups.Password);
        }



        void ValidateOrThrow(string requirement, string groupName)
        {
            if (groups.Any(p => p.Equals(groupName)) == false)
                throw new InvalidOperationException(
                    $"Invalid scheme. {requirement} set to Required, but no group '{groupName}' present in Regex");
        }
    }


    public ProxyData Parse(string input)
    {
        return Parse(input, false)!;
    }


    public bool TryParse(string input, [NotNullWhen(true)] out ProxyData? result)
    {
        result = Parse(input, true);
        return result != null;
    }

    private ProxyData? Parse(string input, bool trying)
    {
        var match = Regex.Match(input);



        if (match.Success == false)
        {
            if (trying) return null;
            throw new FormatException("Provided string doesn't match pattern");
        }

        var parsed = ParseProtocol(match, trying);
        if (parsed == null) return null;
        var protocol = parsed.Value;


        var host = ParseHost(match, trying);
        var port = ParsePort(match, trying);
        if (host == null || port == null) return null;


        var username = ParseCredential(match, SchemeGroups.Username);
        string? password = null;
        if (UsernameRequirement == PatternRequirement.Required && username == null)
        {
            if (trying) return null;
            throw new FormatException(
                "Error while parsing ProxyData. Username is required but was not presented in input string");
        }

        if (username != null)
        {
            var pass = ParseCredential(match, SchemeGroups.Password);
            if (PasswordRequirement == PatternRequirement.Required && pass == null)
            {
                if (trying) return null;
                throw new FormatException(
                    "Error while parsing ProxyData. Password is required but was not presented in input string");
            }

            password = pass;
        }



        return new ProxyData(protocol, host, port.Value, username, password);
    }

    private ProxyProtocol? ParseProtocol(Match match, bool trying)
    {
        var group = match.Groups[SchemeGroups.Protocol];
        string? protocolString = null;
        if (group.Success)
        {
            protocolString = group.Value;
        }

        if (ProtocolRequired && protocolString == null)
        {
            if (trying) return null;
            throw new FormatException($"Error while parsing ProxyData. Protocol required but no protocol specified in input");
        }

        if (protocolString != null)
        {
            if (AllowedProtocols
                    .GetFlags()
                    .Any(p => protocolString
                        .Equals(p.ToString(), StringComparison.InvariantCultureIgnoreCase)) == false)
            {
                if (trying) return null;
                throw new FormatException($"Provided protocol {protocolString} is not meeting allowed scheme");
            }

            return Enum.Parse<ProxyProtocol>(protocolString, true);

        }
        return DefaultProtocol;


    }


    private string? ParseHost(Match match, bool trying)
    {
        var group = match.Groups[SchemeGroups.Host];
        if (group.Success == false)
        {
            if (trying) return null;
            throw new FormatException("Error while parsing ProxyData. Host is not specified");
        }

        var input = group.Value;

        var parsed = Uri.CheckHostName(input);
        if (parsed == UriHostNameType.Unknown)
        {
            if (trying) return null;
            throw new FormatException($"Error while parsing ProxyData. Host '{parsed}' format of  is Unknown");
        };



        if (AllowedFormats.GetFlags().Any(format => Validate(format, parsed)))
        {
            return input;
        }

        if (trying) return null;
        throw new FormatException($"Error while parsing ProxyData. Provided Host '{parsed}' was in not allowed format");


        static bool Validate(ProxyPatternHostFormat format, UriHostNameType parsedType)
        {
            return format switch
            {
                ProxyPatternHostFormat.IPv4 => parsedType == UriHostNameType.IPv4,
                ProxyPatternHostFormat.IPv6 => parsedType == UriHostNameType.IPv6,
                ProxyPatternHostFormat.Domain => parsedType == UriHostNameType.Dns,

                _ => false,
            };
        }
    }

    private int? ParsePort(Match match, bool trying)
    {
        var group = match.Groups[SchemeGroups.Port];
        if (group.Success == false)
        {
            if (trying) return null;
            throw new FormatException("Error while parsing ProxyData. Port is not specified");
        }

        var input = group.Value;
        if (int.TryParse(input, out var port) && port is >= 0 and <= 65535) return port;


        if (trying) return null;
        throw new FormatException($"Error while parsing ProxyData. Provided port {input} is not valid Port");
    }

    private string? ParseCredential(Match match, string groupName)
    {
        var group = match.Groups[groupName];
        return group.Success ? group.Value : null;
    }


}