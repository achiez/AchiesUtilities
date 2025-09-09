namespace AchiesUtilities.Web.Proxy;

/// <summary>
///     A stricter version of <see cref="DynamicProxy" /> that enforces the presence of proxy settings.
/// </summary>
/// <remarks>
///     Unlike <see cref="DynamicProxy" />, this class guarantees that a request cannot be sent
///     without a configured proxy. If <see cref="ProxyData" /> is <c>null</c> and a request attempts
///     to resolve a proxy via <see cref="GetProxy(Uri)" />, an exception will be thrown.
///     An optional <see cref="Func{TArg,TResult}" /> (<c>exceptionFactory</c>) can be supplied to customize
///     the exception type or message. If omitted, <see cref="InvalidOperationException" /> is thrown.
/// </remarks>
/// <example>
///     <code>
/// var proxy = new MandatoryDynamicProxy(exceptionFactory: () => 
///     new InvalidOperationException("Proxy configuration is required"));
/// 
/// // Throws if no proxy has been set:
/// proxy.GetProxy(new Uri("http://example.com"));
/// </code>
/// </example>
public class MandatoryDynamicProxy : DynamicProxy
{
    private readonly Func<Uri, Exception>? _exceptionFactory;

    public MandatoryDynamicProxy(ProxyData? data = null, Func<Uri, Exception>? exceptionFactory = null) : base(data)
    {
        _exceptionFactory = exceptionFactory;
    }

    public MandatoryDynamicProxy(Func<Uri, Exception>? exceptionFactory)
    {
        _exceptionFactory = exceptionFactory;
    }


    public override Uri? GetProxy(Uri destination)
    {
        if (IsBypassed(destination)) return null;
        var res = base.GetProxy(destination);
        if (res == null)
        {
            var customException = _exceptionFactory?.Invoke(destination);
            if (customException != null) throw customException;
            throw new InvalidOperationException("Proxy is not set");
        }

        return res;
    }
}