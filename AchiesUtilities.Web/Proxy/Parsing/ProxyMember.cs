namespace AchiesUtilities.Web.Proxy.Parsing;

public class ProxyMember
{
    public string Name { get; set; }

}

public enum ProxyMemberType
{
    Protocol, Host, Port, Username, Password
}