namespace AchiesUtilities.Web.Proxy.Parsing;

[Flags]
public enum ProxyPatternHostFormat
{
    IPv4 = 1,
    IPv6 = 2,
    Domain = 4,
    All = 7
}