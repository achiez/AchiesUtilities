namespace AchiesUtilities.Web.Extensions;

public static class HttpResponseMessageExtension
{
    public static string ReadAsStringSync(this HttpResponseMessage message)
    {
        return message.Content.ReadAsStringSync();
    }


    public static string ReadAsStringEnsureSuccessSync(this HttpResponseMessage message,
        CancellationToken cancellationToken = default)
    {
        return message.EnsureSuccessStatusCode().Content.ReadAsStringSync();
    }

    public static Task<string> ReadAsStringAsync(this HttpResponseMessage message,
        CancellationToken cancellationToken = default)
    {
        return message.Content.ReadAsStringAsync(cancellationToken);
    }

    public static Task<string> ReadAsStringEnsureSuccessAsync(this HttpResponseMessage message,
        CancellationToken cancellationToken = default)
    {
        return message.EnsureSuccessStatusCode().Content.ReadAsStringAsync(cancellationToken);
    }


    //public static CookieCollection ExtractCookies(this HttpResponseMessage message)
    //{
    //    var cookies = new CookieCollection();
    //    if (message.Headers.TryGetValues("Set-Cookie", out var setCookies) == false)
    //    {
    //        return cookies;
    //    }

    //    foreach (var cookie in setCookies)
    //    {
    //        var container = new CookieContainer();
    //        container.SetCookies();
    //    }


    //}
}