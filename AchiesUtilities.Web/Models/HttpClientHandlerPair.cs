using JetBrains.Annotations;

namespace AchiesUtilities.Web.Models;

[PublicAPI]
public struct HttpClientHandlerPair
{
    public HttpClient Client { get; }
    public HttpClientHandler Handler { get; }

    public HttpClientHandlerPair(HttpClient client, HttpClientHandler handler)
    {
        Client = client;
        Handler = handler;
    }
}