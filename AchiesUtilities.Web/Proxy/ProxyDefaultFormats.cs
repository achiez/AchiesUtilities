using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace AchiesUtilities.Web.Proxy;

public static class ProxyDefaultFormats
{
    [RegexPattern]
    private static readonly string _protocol =
        $"(?<{ProxyScheme.SchemeGroups.Protocol}>{WebRegexPatterns.ProtocolPattern})";

    [RegexPattern]
    private static readonly string _proxyHost = $@"(?<{ProxyScheme.SchemeGroups.Host}>"
                                                + $"(?:{WebRegexPatterns.IPv4Pattern})|"
                                                + $"(?:{WebRegexPatterns.DomainNamePattern})|"
                                                + "(?:localhost)"
                                                + ')';
    [RegexPattern]
    private static readonly string _port = @"(?<" + $"{ProxyScheme.SchemeGroups.Port}" + @">\d{1,5})"; //Not the best, but its better than full validation

    [RegexPattern]
    private static readonly string _user = $"(?<{ProxyScheme.SchemeGroups.Username}>.+)";

    [RegexPattern]
    private static readonly string _pass = $"(?<{ProxyScheme.SchemeGroups.Password}>.+)";


    /// <summary>
    /// <c>{PROTOCOL://}:{HOST}:{PORT}:{USER}:{PASS}</c><br/>
    /// Protocol and USER:PASS are optional
    /// </summary>
    public static readonly Regex UniversalHostFirstColonDelimiter = new(
                "^" + @$"(?:{_protocol}://)?"
                    + _proxyHost
                    + ":"
                    + _port
                    + "(?::" + _user + ":" + _pass + ")?"
                    + "[^:]$");


    /// <summary>
    /// <c>{PROTOCOL://}{USER}:{PASS}@{HOST}:{PORT}:</c><br/>
    /// Protocol and USER:PASS are optional
    /// </summary>
    public static readonly Regex UniversalHostFirstSignAtDelimiter = new(
        "^" + @$"(?:{_protocol}://)?"
            + "(?:" + _user + ":[^/]" + _pass + ")?"
            + "@"
            + _proxyHost
            + ":"
            + _port
            + "$");

}