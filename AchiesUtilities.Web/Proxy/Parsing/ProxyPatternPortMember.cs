namespace AchiesUtilities.Web.Proxy.Parsing;

public class ProxyPatternPortMember : ProxyPatternMember
{
    public override PatternRequirement Requirement => PatternRequirement.Required;
    public override bool Validate(string input)
    {
        return int.TryParse(input, out var port) && port is >= 0 and <= 65353;
    }

    public ProxyPatternPortMember() : base(ProxyPattern.ReservedWords.Port)
    { }
}