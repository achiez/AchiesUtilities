namespace AchiesUtilities.Web.Proxy.Parsing;

[Flags]
public enum ProxyPatternProtocol
{
    HTTP = 1, HTTPs = 2, SOCKS4 = 4, SOCKS5 = 8, All = 15
}