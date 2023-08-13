using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace AchiesUtilities.Web.Proxy.Parsing;


[Obsolete("Unsuccessful implementation")]
public class ProxyPattern
{
    internal static class ReservedWords
    {
        public static readonly string Protocol = "PROTOCOL";
        public static readonly string Host = "HOST";
        public static readonly string Port = "PORT";
        public static readonly string Username = "USER";
        public static readonly string Password = "PASSWORD";

        internal static string GetReservedDefinition(string str)
        {
            return '{' + str + '}';
        }
    }

    private static readonly IReadOnlyList<Type> RequiredMembers = new ReadOnlyCollection<Type>(new[]
    {
        typeof(ProxyPatternHostMember),
        typeof(ProxyPatternProtocolMember),
        typeof(ProxyPatternPortMember)
    });
 

    public IReadOnlyList<ProxyPatternMember> Members { get; }

    public Regex PatternRegex { get; }

    public ProxyPattern(IEnumerable<ProxyPatternMember> members)
    {
        List<Type> reservedTypes = new();
        var leftRequired = RequiredMembers.ToList();
        members = members.ToList();
        foreach (var member in members)
        {
            var type = member.GetType();
            var isString = type == typeof(ProxyStringMember);
            if (!isString && reservedTypes.Contains(type))
            {
                throw new InvalidOperationException("Proxy pattern can't contain more than one " + type.Name);
            }

            if (!isString)
            {
                reservedTypes.Add(type);
            }

            leftRequired.Remove(type);
        }

        if (leftRequired.Any())
        {
            var errorJoin = string.Join(", ", leftRequired.Select(t => t.Name));
            throw new InvalidOperationException($"Proxy pattern must contain " + errorJoin);
        }

        Members = new ReadOnlyCollection<ProxyPatternMember>(members.ToList());
        PatternRegex = Create(members);
    }

    private static Regex Create(IEnumerable<ProxyPatternMember> members)
    {
        var sb = new StringBuilder("^");

        foreach (var member in members)
        {
            sb.Append('(');
            sb.Append('?');
            if (member is ProxyStringMember str)
            {
                sb.Append('>');
                sb.Append(Regex.Escape(str.Word));

            }
            else if (member is ProxyPatternProtocolMember)
            {
                sb.Append('<');
                sb.Append(member.Word);
                sb.Append('>');
                sb.Append("[a-z]\\w+");

            }else if (member is ProxyPatternPortMember)
            {
                sb.Append('<');
                sb.Append(member.Word);
                sb.Append('>');
                sb.Append("\\d+");
            }else if (member is ProxyPatternHostMember)
            {
                sb.Append('<');
                sb.Append(member.Word);
                sb.Append('>');
                sb.Append("[a-z0-9\\.-]+");
            }
            else
            {
                sb.Append('<');
                sb.Append(member.Word);
                sb.Append('>');
                sb.Append(".+");

            }

            sb.Append(')');

            if (member.Requirement != PatternRequirement.Required)
            {
                sb.Append('?');
            }
        }
        sb.Append('$');

        return new Regex(sb.ToString());
    }

    public ProxyData Parse(string input)
    {
        return Parse(input, false)!;
    }

    public bool TryParse(string input, [NotNullWhen(true)]out ProxyData? proxyData)
    {
        proxyData = Parse(input, true);
        return proxyData != null;
    }



    private ProxyData? Parse(string input, bool trying)
    {
        var match = PatternRegex.Match(input);



        if (match.Success == false)
        {
            if (trying) return null;
            throw new FormatException("Provided string doesn't match pattern");
        }



        var protocolPattern = (ProxyPatternProtocolMember)Members.First(m => m is ProxyPatternProtocolMember);
        var protocol = ParseProtocol(protocolPattern, match, trying);
        if (protocol == null) return null;

        var hostPattern = (ProxyPatternHostMember) Members.First(m => m is ProxyPatternHostMember);
        var portPattern = (ProxyPatternPortMember) Members.First(m => m is ProxyPatternPortMember);

        var host = ParseRequired(hostPattern, match, trying);
        var port = ParseRequired(portPattern, match, trying);
        if (host == null || port == null) return null;


        var usernamePattern = (ProxyPatternUsernameMember?)Members.FirstOrDefault(m => m is ProxyPatternUsernameMember);
        var passwordPattern = (ProxyPatternPasswordMember?)Members.FirstOrDefault(m => m is ProxyPatternPasswordMember);

        string? username = null;
        string? password = null;
        if (usernamePattern != null)
        {
            var usernameValid = ParseCredentials(usernamePattern, match, trying, out username);
            if (!usernameValid)
            {
                return null;
            }
        }

        if (usernamePattern != null && passwordPattern != null)
        {
            var passwordValid = ParseCredentials(usernamePattern, match, trying, out password);
            if (!passwordValid)
            {
                return null;
            }

        }

        return new ProxyData(protocol.Value, host, int.Parse(port), username, password);
    }

    private ProxyProtocol? ParseProtocol(ProxyPatternProtocolMember protocolPattern, Match match, bool trying)
    {
        var group = match.Groups[protocolPattern.Word];
        string? protocolString = null;
        if (group.Success)
        {
            protocolString = group.Value;
        }

        if (protocolString == null && protocolPattern.Requirement == PatternRequirement.Required)
        {
            if (trying) return null;
            throw new FormatException("Protocol is required and doesn't provided in input string");
        }

        if (protocolString != null) 
        {
            if (protocolPattern.Validate(protocolString) == false)
            {
                if (trying) return null;
                throw new FormatException($"Provided protocol {protocolString} is not meeting allowed scheme");
            }

            return Enum.Parse<ProxyProtocol>(protocolString, true);

        }
        else
        {
            return protocolPattern.Default!.Value; //Not null because there Requirement == Optional and Default set
        }
    }


    private string? ParseRequired(ProxyPatternMember pattern, Match match, bool trying)
    {
        var group = match.Groups[pattern.Word];
        var input = group.Value;


        if (pattern.Validate(input)) return input;

        if (trying) return null;
        throw new FormatException($"Provided {pattern.Word} {input} is not meeting allowed scheme");

    }


    private bool ParseCredentials(ProxyPatternMember pattern, Match match, bool trying, out string? credential)
    {
        var group = match.Groups[pattern.Word];
        credential = null;

        if (group.Success)
        {
            credential = group.Value;
        }

        if (credential == null && pattern.Requirement == PatternRequirement.Required)
        {
            if (trying) return false;
            throw new FormatException($"{pattern.Word} is required and doesn't provided in input string");
        }

        if (credential != null && pattern.Validate(credential) == false)
        {
            if (trying) return false;
            throw new FormatException($"{pattern.Word} value of '{credential}' is not valid for Proxy");
        }
       
        return true;
    }
}