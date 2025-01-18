using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace AchiesUtilities.Web.Proxy.Parsing;

[PublicAPI]
public static class ProxyDefaultFormats
{
    [RegexPattern]
    private static readonly string _protocol =
        $"(?<{ProxyParser.SchemeGroups.Protocol}>{RegexWebPatterns.ProtocolPattern})";

    [RegexPattern]
    private static readonly string _proxyHost = $@"(?<{ProxyParser.SchemeGroups.Host}>"
                                                + $"(?:{RegexWebPatterns.IPv4Pattern})|"
                                                + $"(?:{RegexWebPatterns.DomainNamePattern})|"
                                                + "(?:localhost)"
                                                + ')';
    [RegexPattern]
    private static readonly string _port = @"(?<" + $"{ProxyParser.SchemeGroups.Port}" + @">\d{1,5})"; //Not the best, but it's easier than full validation

    [RegexPattern]
    private static readonly string _user = $"(?<{ProxyParser.SchemeGroups.Username}>.+)";

    [RegexPattern]
    private static readonly string _pass = $"(?<{ProxyParser.SchemeGroups.Password}>.+)";


    /// <summary>
    /// <c>{PROTOCOL://}:{HOST}:{PORT}:{USER}:{PASS}</c><br/>
    /// Protocol and USER:PASS are optional
    /// </summary>
    public static readonly Regex UniversalColon = new(
                "^" + @$"(?:{_protocol}://)?"
                    + _proxyHost
                    + ":"
                    + _port
                    + "(?::" + _user + ":" + _pass + ")?"
                    + "$",
                RegexOptions.Compiled);


    /// <summary>
    /// <c>{PROTOCOL://}{USER}:{PASS}@{HOST}:{PORT}:</c><br/>
    /// Protocol and USER:PASS are optional
    /// </summary>
    public static readonly Regex UniversalSignAt = new(
        "^" + @$"(?:{_protocol}://)?"
            + "(?:" + _user + ":" + _pass + "@)?"
            + _proxyHost
            + ":"
            + _port
            + "$",
        RegexOptions.Compiled);


    public static readonly ProxyParser UniversalColonParser = new(
        UniversalColon,
        false,
        ProxyProtocol.HTTP,
        ProxyPatternProtocol.All,
        ProxyPatternHostFormat.All,
        PatternRequirement.Optional,
        PatternRequirement.Optional
    );

    public static readonly ProxyParser UniversalSignAtParser = new(
        UniversalSignAt,
        false,
        ProxyProtocol.HTTP,
        ProxyPatternProtocol.All,
        ProxyPatternHostFormat.All,
        PatternRequirement.Required,
        PatternRequirement.Required
    );

}