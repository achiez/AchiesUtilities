using AchiesUtilities.Extensions;

namespace AchiesUtilities.Web.Proxy.Parsing;

public class ProxyPatternHostMember : ProxyPatternMember
{
    public override PatternRequirement Requirement => PatternRequirement.Required;

    public ProxyPatternHostFormat AllowedFormats { get; }
    public ProxyPatternHostMember(ProxyPatternHostFormat allowedAllowedFormats) : base(ProxyPattern.ReservedWords.Host)
    {
        AllowedFormats = allowedAllowedFormats;
    }

    public override bool Validate(string input)
    {
        var parsed = Uri.CheckHostName(input);
        if (parsed == UriHostNameType.Unknown) return false;
        foreach (var format in AllowedFormats.GetFlags())
        {
            if (Validate(format, parsed))
                return true;
        }

        return false;
    }


    private bool Validate(ProxyPatternHostFormat format, UriHostNameType parsed)
    {
        return format switch
        {
            ProxyPatternHostFormat.IPv4 => parsed == UriHostNameType.IPv4,
            ProxyPatternHostFormat.IPv6 => parsed == UriHostNameType.IPv6,
            ProxyPatternHostFormat.Domain => parsed == UriHostNameType.Dns,

            _ => false,
        };
    }


}