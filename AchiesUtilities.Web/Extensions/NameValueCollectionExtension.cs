using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace AchiesUtilities.Web.Extensions;

public static class NameValueCollectionExtension
{
    public static string ToQueryString(this IEnumerable<KeyValuePair<string, string>> collection, bool encode = true)
    {
        var sb = new StringBuilder("?");
        foreach (var kvp in collection)
        {
            var key = encode ? HttpUtility.UrlEncode(kvp.Key) : kvp.Key;
            var value = encode ? HttpUtility.UrlEncode(kvp.Value) : kvp.Value;
            sb.Append(key);
            sb.Append('=');
            sb.Append(value);
            sb.Append('&');
        }

        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }

    public static string ToQueryString(this NameValueCollection nvc, bool encode = true)
    {
        var sb = new StringBuilder("?");
        foreach (var key in nvc.AllKeys)
        {
            var values = nvc.GetValues(key);
            if (values == null) continue;
            var keyV = encode ? HttpUtility.UrlEncode(key) : key;
            foreach (var value in values)
            {
                var valueV = encode ? HttpUtility.UrlEncode(value) : value;
                sb.Append(keyV);
                sb.Append('=');
                sb.Append(valueV);
                sb.Append('&');
            }
        }

        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }
}