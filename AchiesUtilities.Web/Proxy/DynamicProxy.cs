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

    private readonly ProxyCredentials _credentials = new();

    public DynamicProxy(ProxyData? data = null)
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
            _credentials.Set(_data.Username, _data.Password);
        }
        else
        {
            _credentials.Reset();
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


internal class ProxyCredentials : ICredentials
{
    private string? _username;
    private string? _password;

    private NetworkCredential Credential => _credential ??= new NetworkCredential(_username, _password);
    private NetworkCredential? _credential;
    private bool _isSet;

    public ProxyCredentials(){}

    public ProxyCredentials(string? username, string? password)
    {
        if (username != null && password != null)
        {
            _isSet = true;
            _username = username;
            _password = password;
        }
    }

    public void Set(string username, string password)
    {
        if (username != _username || password != _password)
        {
            _credential = null;
        }
        _username = username;
        _password = password;
        _isSet = true;
    }

    public void Reset()
    {
        _username = null;
        _password = null;
        _isSet = false;
    }

    public NetworkCredential? GetCredential(Uri uri, string authType)
    {
        return _isSet ? Credential : null;
    }
}