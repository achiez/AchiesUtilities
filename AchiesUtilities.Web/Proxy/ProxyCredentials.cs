using System.Net;

namespace AchiesUtilities.Web.Proxy;

internal class ProxyCredentials : ICredentials
{
    private NetworkCredential Credential => _credential ??= new NetworkCredential(_username, _password);
    private NetworkCredential? _credential;
    private bool _isSet;
    private string? _password;
    private string? _username;

    public ProxyCredentials()
    {
    }

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