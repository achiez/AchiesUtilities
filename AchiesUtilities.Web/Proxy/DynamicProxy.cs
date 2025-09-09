using System.Net;
using JetBrains.Annotations;

namespace AchiesUtilities.Web.Proxy;

/// <summary>
///     Represents a dynamic proxy implementation of <see cref="IWebProxy" /> that allows
///     changing proxy configuration on the fly without recreating or disposing the underlying
///     <see cref="HttpClientHandler" /> or <see cref="SocketsHttpHandler" />.
/// </summary>
/// <remarks>
///     By design, <see cref="HttpClientHandler.Proxy" /> becomes immutable after the first request,
///     which prevents changing proxy settings at runtime. <see cref="DynamicProxy" /> bypasses this
///     limitation by acting as an indirection layer: <see cref="ProxyData" /> can be reassigned
///     at any time through <see cref="SetData" />, and the proxy address or credentials will be updated
///     transparently.
///     Internally, it uses a custom <see cref="ICredentials" /> implementation (<c>ProxyCredentials</c>)
///     to avoid .NET credential caching. This makes it possible to reuse the same proxy server
///     with different credentials.
/// </remarks>
/// <example>
///     Typical usage with <see cref="HttpClientHandler" />:
///     <code>
/// var proxy = new DynamicProxy(new ProxyData("http://my.proxy:8080", "user", "pass"));
/// var handler = new HttpClientHandler
/// {
///     Proxy = proxy,
///     UseProxy = true
/// };
/// 
/// var client = new HttpClient(handler);
/// 
/// // Later, update proxy data without recreating the handler:
/// proxy.SetData(new ProxyData("http://my.proxy:8080", "newUser", "newPass"));
/// </code>
/// </example>
[PublicAPI]
public class DynamicProxy : IDynamicProxy
{
    public Uri? Address { get; protected set; }

    public ProxyData? Data
    {
        get => _data;
        set => SetData(value);
    }

    public HashSet<string> BypassHosts { get; init; } = [];

    /// <summary>
    ///     Property is readonly
    /// </summary>
    public ICredentials? Credentials
    {
        get => _credentials;
        set => throw new MemberAccessException("Credentials property of DynamicProxy is readonly");
    }

    private readonly ProxyCredentials _credentials = new();
    private ProxyData? _data;

    public DynamicProxy(ProxyData? data = null)
    {
        SetData(data);
    }

    public DynamicProxy()
    {
    }

    public virtual ProxyData? GetData()
    {
        return Data;
    }

    public void SetData(ProxyData? data)
    {
        _data = data;
        if (_data == null)
        {
            Address = null;
            return;
        }

        var address = _data.ToString();
        Address = new Uri(address);
        if (_data.AuthEnabled)
        {
            _credentials.Set(_data.Username, _data.Password);
        }
        else
        {
            _credentials.Reset();
        }
    }

    public virtual Uri? GetProxy(Uri destination)
    {
        return IsBypassed(destination) ? destination : Address;
    }

    public virtual bool IsBypassed(Uri host)
    {
        return BypassHosts.Contains(host.Host);
    }

    public virtual void AddToBypass(Uri host)
    {
        BypassHosts.Add(host.Host);
    }
}