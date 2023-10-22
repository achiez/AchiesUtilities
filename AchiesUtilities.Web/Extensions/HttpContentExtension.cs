namespace AchiesUtilities.Web.Extensions;

public static class HttpContentExtension
{
    public static string ReadAsStringSync(this HttpContent content)
    {
        var stream = new StreamReader(content.ReadAsStream());
        try
        {
            var result = stream.ReadToEnd();
            stream.Dispose();
            return result;
        }
        finally
        {
            stream.Dispose();
        }
    }
}