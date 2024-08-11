using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using AchiesUtilities.Extensions;
using JetBrains.Annotations;

namespace AchiesUtilities.Web.Proxy;

[PublicAPI]
public class ProxyData
{
    public ProxyProtocol Protocol { get; }
    public string Address { get; }
    public int Port { get; }
    public string? Username { get; }
    public string? Password { get; }

    [MemberNotNullWhen(true, nameof(Username), nameof(Password))]
    public bool AuthEnabled => Username != null && Password != null;

    public ProxyData(ProxyProtocol protocol, string address, int port, string? username = null, string? password = null)
    {
        Protocol = protocol;
        Address = address;
        Port = port;
        Username = username;
        Password = password;
    }

    /// <summary>
    /// Patterns: <c>{IP} {PORT}</c>. Optional: <c>{USER} {PASS} {TYPE}</c><br/>
    /// Default type HTTP if Type not parsed automatically or from pattern
    /// </summary>
    /// <param name="str"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="InvalidOperationException"></exception>

    [Obsolete("Use ProxyScheme")]
    public static ProxyData Parse(string str, string pattern)
    {
        if (!Contains(pattern, "{IP}") ||
            !Contains(pattern, "{PORT}"))
        {
            throw new FormatException("Pattern must contain {IP} and {PORT} values");
        }

        pattern = Replace(pattern, "{IP}", "(?<ip>\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3})");
        pattern = Replace(pattern, "{PORT}", "(?<port>\\d{1,5})");
        bool hasCred;
        bool hasType = false;
        if (Contains(pattern, "{USER}"))
        {
            hasCred = true;
            if (Contains(pattern, "{PASS}") == false)
                throw new FormatException("Pattern has {USER} but not {PASS}.");

            pattern = Replace(pattern, "{USER}", "(?<user>\\w+)");
            pattern = Replace(pattern, "{PASS}", "(?<pass>\\w+)");
        }
        else
        {
            hasCred = false;
        }

        if (Contains(pattern, "{TYPE}"))
        {
            hasType = true;
            pattern = Replace(pattern, "{TYPE}", "(?<type>\\w+)");

        }

        var match = Regex.Match(str, pattern);
        if (match.Success == false)
            throw new FormatException("Pattern does not matches provided string");

        var ip = match.Groups["ip"].Value;
        var port = Convert.ToInt32(match.Groups["port"].Value);
        string? user = null;
        string? pass = null;
        var type = ProxyProtocol.HTTP;
        if (hasCred)
        {
            user = match.Groups["user"].Value;
            pass = match.Groups["pass"].Value;
        }

        if (hasType)
        {
            var typeMatch = match.Groups["type"].Value;
            type = Enum.Parse<ProxyProtocol>(typeMatch, true);
        }
        else
        {
            if (Contains(str, "socks5"))
            {
                type = ProxyProtocol.SOCKS5;
            }
            else if (Contains(str, "socks4"))
            {
                type = ProxyProtocol.SOCKS4;
            }
        }


        return new ProxyData(type, ip, port, user, pass);
    }

    /// <summary>
    /// Patterns: <c>{IP} {PORT}</c>. Optional: <c>{USER} {PASS} {TYPE}</c>
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public string ToString(string pattern)
    {
        pattern = Replace(pattern, "{IP}", Address);
        pattern = Replace(pattern, "{PORT}", Port.ToString());

        if (ValidateHasCredentials())
        {
            pattern = Replace(pattern, "{USER}", Username);
            pattern = Replace(pattern, "{PASS}", Password);
        }

        if (Contains(pattern, "{TYPE}"))
        {
            pattern = Replace(pattern, "{TYPE}", Protocol.ToString().ToLower());
        }

        return pattern;
    }

    public override string ToString()
    {
        return ToString("{TYPE}://{IP}:{PORT}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [MemberNotNullWhen(true, nameof(Username))]
    [MemberNotNullWhen(true, nameof(Password))]
    public bool ValidateHasCredentials()
    {
        if (Username == null) return false;
        if (Password == null)
            throw new InvalidOperationException("Proxy contains username but not password");
        return true;
    }

    private static string Replace(string input, string toReplace, string value)
    {
        return input.ReplaceIgnoreCase(toReplace, value);
    }

    private static bool Contains(string input, string value)
    {
        return input.ContainsIgnoreCase(value);
    }

    public override bool Equals(object? obj)
    {
        return obj is ProxyData p && Equals(p);
    }

    public bool Equals(ProxyData other)
    {
        return Protocol == other.Protocol
               && Address == other.Address
               && Port == other.Port
               && Username == other.Username
               && Password == other.Password;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int) Protocol, Address, Port, Username, Password);
    }

}