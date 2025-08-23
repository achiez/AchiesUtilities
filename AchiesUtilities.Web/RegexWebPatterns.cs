using JetBrains.Annotations;

namespace AchiesUtilities.Web;

public class RegexWebPatterns
{
    [RegexPattern] public static readonly string DomainNamePattern =
        @"(?!-)(?:[A-Za-z0-9]{1,63}(?:-[A-Za-z0-9]{1,62})?\.)+[A-Za-z]{2,63}";

    [RegexPattern] public static readonly string IPv4Pattern =
        @"(?:(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)\.){3}(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)";

    [RegexPattern] public static readonly string ProtocolPattern = "(?:(?i)(?:https|http|socks5|socks4))";
}