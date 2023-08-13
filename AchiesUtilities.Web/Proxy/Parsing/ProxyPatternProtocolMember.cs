using AchiesUtilities.Extensions;

namespace AchiesUtilities.Web.Proxy.Parsing;

public class ProxyPatternProtocolMember : ProxyPatternMember
{
    public override PatternRequirement Requirement { get; }
    public ProxyPatternProtocol AllowedProtocols { get; }
    public ProxyProtocol? Default { get; }


    /// <summary>
    /// Represents <see cref="ProxyProtocol"/> pattern member. This Constructor sets <see cref="Requirement"/> to <see cref="PatternRequirement.Required"/>
    /// </summary>
    /// <param name="allowedProtocols"></param>
    public ProxyPatternProtocolMember(ProxyPatternProtocol allowedProtocols) : base(ProxyPattern.ReservedWords.Protocol)
    {
        AllowedProtocols = allowedProtocols;
        Requirement = PatternRequirement.Required;
    }

    /// <summary>
    /// Represents <see cref="ProxyProtocol"/> pattern member. This Constructor sets <see cref="Requirement"/> to <see cref="PatternRequirement.Optional"/>
    /// </summary>
    /// <param name="allowedProtocols"></param>
    /// <param name="defaultProtocol">Protocol value which will will be used if protocol not specified in pattern</param>
    public ProxyPatternProtocolMember(ProxyPatternProtocol allowedProtocols, ProxyProtocol defaultProtocol) : base(ProxyPattern.ReservedWords.Protocol)
    {
        AllowedProtocols = allowedProtocols;
        Default = defaultProtocol;
        Requirement = PatternRequirement.Optional;
    }

    public override string ToString()
    {
        return ProxyPattern.ReservedWords.Protocol;
    }

    public override bool Validate(string input)
    {
        return AllowedProtocols.GetFlags()
            .Any(p => input.Equals(p.ToString(), StringComparison.InvariantCultureIgnoreCase));

    }
}