namespace AchiesUtilities.Web.Proxy.Parsing;

[Obsolete("Unsuccessful implementation")]
public abstract class ProxyPatternMember
{
    public string Word { get; }
    public abstract PatternRequirement Requirement { get; }
    protected ProxyPatternMember(string word)
    {
        Word = word;
    }

    public override string ToString()
    {
        return ProxyPattern.ReservedWords.GetReservedDefinition(Word);
    }

    public abstract bool Validate(string input);
}