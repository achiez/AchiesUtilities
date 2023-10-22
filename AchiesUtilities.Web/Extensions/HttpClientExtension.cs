namespace AchiesUtilities.Web.Extensions;

public static class HttpClientExtension
{
    public static string CheckConnectionDefaultUrl { get; set; } = "http://www.gstatic.com/generate_204";

    public static Task<bool> CheckConnection(this HttpClient client)
    {
        return CheckConnection(client, CheckConnectionDefaultUrl);
    }
    public static async Task<bool> CheckConnection(this HttpClient client, string url)
    {
        try
        {
            var resp = await client.GetAsync(url);
            return true;
        }
        catch
        {
            return false;
        }
    }
}