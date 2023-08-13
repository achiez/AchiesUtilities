namespace AchiesUtilities.Web.Proxy.Parsing;

public class ProxyPatternPasswordMember : ProxyPatternMember
{
    public override PatternRequirement Requirement { get; }
    public ProxyPatternPasswordMember(PatternRequirement requirement) : base(ProxyPattern.ReservedWords.Password)
    {
        Requirement = requirement;
    }
    public override bool Validate(string input)
    {
        if (Requirement == PatternRequirement.Required)
        {
            return string.IsNullOrWhiteSpace(input) == false;
        }

        return true;
    }
}