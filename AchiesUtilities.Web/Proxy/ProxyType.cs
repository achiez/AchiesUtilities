namespace AchiesUtilities.Web.Proxy;

public enum ProxyProtocol
{
    HTTP,
    [Obsolete("Not supported in .NET")]
    HTTPS, 
    SOCKS4, 
    SOCKS5
}
