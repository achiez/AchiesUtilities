namespace AchiesUtilities.Web.Proxy.Parsing;

public class ProxyPatternUsernameMember : ProxyPatternMember
{
    public override PatternRequirement Requirement { get; }

    public ProxyPatternUsernameMember(PatternRequirement requirement) : base(ProxyPattern.ReservedWords.Username)
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