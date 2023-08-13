namespace AchiesUtilities.Web.Proxy.Parsing;

public class ProxyStringMember : ProxyPatternMember
{
    public override PatternRequirement Requirement { get; }
    public StringComparison Comparison { get; }
    public ProxyStringMember(string word, StringComparison comparison = StringComparison.CurrentCulture, PatternRequirement requirement = PatternRequirement.Required) : base(word)
    {
        Requirement = requirement;
        Comparison = comparison;

    }

    public override string ToString()
    {
        return Word;
    }

    public override bool Validate(string input)
    {
        return input.Equals(input, Comparison);
    }
}