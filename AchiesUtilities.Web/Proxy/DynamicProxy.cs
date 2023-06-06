using JetBrains.Annotations;
using System.Net;

namespace AchiesUtilities.Web.Proxy;

[PublicAPI]
public class DynamicProxy : IWebProxy
{

    public Uri? Address { get; private set; }
    public ProxyData? Data
    {
        get => _data;
        set => SetData(value);
    }
    private ProxyData? _data;
    public HashSet<string> BypassHosts { get; init; } = new();

    /// <summary>
    /// Property is readonly
    /// </summary>
    public ICredentials? Credentials
    {
        get => _credentials;
        set => throw new MemberAccessException("Credentials property of DynamicProxy is readonly");
    }

    private ICredentials? _credentials;

    public DynamicProxy(ProxyData? data)
    {
        SetData(data);
    }

    public void SetData(ProxyData? data)
    {
        _data = data;
        if (_data == null)
        {
            Address = null;
            return;
        };

        var address = _data.ToString();
        Address = new Uri(address);
        if (_data.AuthEnabled)
        {
            _credentials = new NetworkCredential(_data.Username, _data.Password);
        }
    }

    public Uri? GetProxy(Uri destination)
    {
        return IsBypassed(destination) ? destination : Address;
    }

    public bool IsBypassed(Uri host)
    {
        return BypassHosts.Contains(host.Host);
    }

    public void AddToBypass(Uri host)
    {
        BypassHosts.Add(host.Host);
    }
}