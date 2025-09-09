using System.Net;

namespace AchiesUtilities.Web.Proxy;

public interface IDynamicProxy : IWebProxy
{
    public ProxyData? GetData();
    public void SetData(ProxyData? data);
}