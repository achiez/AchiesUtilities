using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using AchiesUtilities.Extensions;
using JetBrains.Annotations;

namespace AchiesUtilities.Web.Proxy.Parsing;

[PublicAPI]
public class ProxyParser
{
    /// <summary>
    /// When testing different proxies using the HTTPs protocol, I received <see cref="AuthenticationException"/> exceptions. I still couldn’t figure out what was causing this error, so using HTTPs is essentially impossible. The best solution would be to use HTTP instead and this variable tells <see cref="ProxyParser.Parse"/> to automatically replace HTTPs with HTTP
    /// </summary>
    public static bool UseOnlyHTTP { get; set; } = true;
    public static class SchemeGroups
    {
        public const string Protocol = "PROTOCOL";
        public const string Host = "HOST";
        public const string Port = "PORT";
        public const string Username = "USER";
        public const string Password = "PASSWORD";
    }

    public Regex Regex { get; }
    public bool ProtocolRequired { get; }
    public ProxyProtocol? DefaultProtocol { get; }
    public ProxyPatternProtocol AllowedProtocols { get; }
    private readonly ProxyPatternProtocol[] _allowedProtocols;
    public ProxyPatternHostFormat AllowedFormats { get; }

    public PatternRequirement UsernameRequirement { get; }
    public PatternRequirement PasswordRequirement { get; }

    /// <summary>
    /// If set to true, the parser will allow empty passwords when Username is present
    /// </summary>
    public bool AllowEmptyPassword { get; }

    public ProxyParser(Regex regex, bool protocolRequired, ProxyProtocol? defaultProtocol, ProxyPatternProtocol allowedProtocols, ProxyPatternHostFormat allowedFormats, PatternRequirement usernameRequirement, PatternRequirement passwordRequirement, bool allowEmptyPassword = false)
    {
        Regex = regex;
        ProtocolRequired = protocolRequired;
        DefaultProtocol = defaultProtocol;
        AllowedProtocols = allowedProtocols;
        _allowedProtocols = AllowedProtocols.GetFlags().ToArray();
        AllowedFormats = allowedFormats;
        UsernameRequirement = usernameRequirement;
        PasswordRequirement = passwordRequirement;
        AllowEmptyPassword = allowEmptyPassword;
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

        if (AllowEmptyPassword == false && PasswordRequirement == PatternRequirement.Ignore)
        {
            throw new InvalidOperationException(
                               $"Invalid scheme. {nameof(AllowEmptyPassword)} set to false, but {nameof(PasswordRequirement)} set to Ignore");
        }

        return;

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
        if (host == null)
        {
            if (trying) return null;
            throw new FormatException("Error while parsing ProxyData. Host is not specified");
        }

        if (port == null)
        {
            if (trying) return null;
            throw new FormatException("Error while parsing ProxyData. Port is not specified");
        }   

        string? username = null;
        string? password = null;

        if (UsernameRequirement != PatternRequirement.Ignore)
        {
            username = ParseCredential(match, SchemeGroups.Username);
            password = null;
            if (UsernameRequirement == PatternRequirement.Required && username == null)
            {
                if (trying) return null;
                throw new FormatException(
                    "Error while parsing ProxyData. Username is required but was not presented in input string");
            }

            if (username != null && PasswordRequirement != PatternRequirement.Ignore)
            {
                var pass = ParseCredential(match, SchemeGroups.Password);
                if ((AllowEmptyPassword == false || PasswordRequirement == PatternRequirement.Required) && pass == null)
                {
                    if (trying) return null;
                    throw new FormatException(
                        "Error while parsing ProxyData. Password is required but was not presented in input string");
                }

                password = pass;
            }
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
            if (_allowedProtocols.All(p => protocolString.EqualsIgnoreCase(p.ToString()) == false))
            {
                if (trying) return null;
                throw new FormatException($"Provided protocol {protocolString} is not meeting allowed scheme");
            }


            var result = Enum.Parse<ProxyProtocol>(protocolString, true);
            if(result == ProxyProtocol.HTTPs && UseOnlyHTTP)
                return ProxyProtocol.HTTP;
            
            return result;

        }

        if (UseOnlyHTTP && DefaultProtocol == ProxyProtocol.HTTPs)
            return ProxyProtocol.HTTP;

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
            throw new FormatException($"Error while parsing ProxyData. Host '{parsed}' format is Unknown");
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