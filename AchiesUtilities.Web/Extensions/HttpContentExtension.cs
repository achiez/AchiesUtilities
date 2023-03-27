namespace AchiesUtilities.Web.Extensions;

public static class HttpContentExtension
{
    public static string ReadAsStringSync(this HttpContent content)
    {
        var stream = new StreamReader(content.ReadAsStream());
        var result = stream.ReadToEnd();
        stream.Dispose();
        return result;
    }
}