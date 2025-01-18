using System.Net;
using AchiesUtilities.Web.Proxy;
using JetBrains.Annotations;

namespace AchiesUtilities.Web.Models;

[PublicAPI]
public class StatefulHttpClient : HttpClient
{
    public CookieContainer CookieContainer { get; }
    public IWebProxy Proxy { get; }

  
    /// <summary>Initializes a new instance of the <see cref="StatefulHttpClient" /> class with the specified handler. The handler is disposed when this instance is disposed.</summary>
    /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="handler" /> is <see langword="null" />.</exception>
    public StatefulHttpClient(HttpMessageHandler handler)
        : base(handler, true)
    {
        if (handler is SocketsHttpHandler sh)
        {
            CookieContainer = sh.CookieContainer;
            Proxy = sh.Proxy ??= new DynamicProxy();
        }
        if(handler is HttpClientHandler hh)
        {
            CookieContainer = hh.CookieContainer;
            Proxy = hh.Proxy ??= new DynamicProxy();
        }

        throw new ArgumentException("Handler must be HttpClientHandler or SocketsHttpHandler");
    }

    /// <summary>Initializes a new instance of the <see cref="StatefulHttpClient" /> class with the provided handler, and specifies whether that handler should be disposed when this instance is disposed.</summary>
    /// <param name="handler">The <see cref="T:System.Net.Http.HttpMessageHandler" /> responsible for processing the HTTP response messages.</param>
    /// <param name="disposeHandler">
    /// <see langword="true" /> if the inner handler should be disposed of by HttpClient.Dispose; <see langword="false" /> if you intend to reuse the inner handler.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="handler" /> is <see langword="null" />.</exception>
    public StatefulHttpClient(HttpMessageHandler handler, bool disposeHandler)
        : base(handler, disposeHandler)
    {
        if (handler is SocketsHttpHandler sh)
        {
            CookieContainer = sh.CookieContainer;
            Proxy = sh.Proxy ??= new DynamicProxy();
        }
        if (handler is HttpClientHandler hh)
        {
            CookieContainer = hh.CookieContainer;
            Proxy = hh.Proxy ??= new DynamicProxy();
        }

        throw new ArgumentException("Handler must be HttpClientHandler or SocketsHttpHandler");
    }
}