namespace AchiesUtilities.Web.Helpers;

public static class ContentHelper
{
    public static FormUrlEncodedContent FormContentFromSingle(string name, string value)
    {
        return new FormUrlEncodedContent(new[] {new KeyValuePair<string, string>(name, value)});
    }
}