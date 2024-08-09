using AchiesUtilities.Web.Proxy.Parsing;

namespace AchiesUtilities.Web.Proxy;

public enum ProxyProtocol
{
    HTTP,
    /// <summary>
    /// <b>Use with caution. For some reason HTTPs appears to be unsupported in the .net and/or windows environment
    /// <br/>
    /// see <see cref="ProxyParser.UseOnlyHTTP"/> for more details</b>
    /// </summary>
    HTTPs, 
    SOCKS4, 
    SOCKS5
}
