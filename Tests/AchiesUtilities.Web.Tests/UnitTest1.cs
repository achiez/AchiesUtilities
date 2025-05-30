using AchiesUtilities.Web.Proxy;
using AchiesUtilities.Web.Proxy.Parsing;

namespace AchiesUtilities.Web.Tests;

[TestFixture]
public class ProxyParserServiceTests
{
    [SetUp]
    public void SetUp()
    {
    }

    private ProxyParser Colon => ProxyDefaultFormats.UniversalColonParser;
    private ProxyParser SignAt => ProxyDefaultFormats.UniversalSignAtParser;


    [TestCase("http://127.0.0.1:8080:user:pass", ProxyProtocol.HTTP, "127.0.0.1", 8080, "user", "pass",
        TestName = "Valid_Full_Format_With_UserPass")]
    [TestCase("127.0.0.1:8080", ProxyProtocol.HTTP, "127.0.0.1", 8080, null, null,
        TestName = "Valid_No_Protocol_No_UserPass")]
    [TestCase("http://127.0.0.1:8080", ProxyProtocol.HTTP, "127.0.0.1", 8080, null, null,
        TestName = "Valid_With_Protocol_No_UserPass")]
    [TestCase("https://domain.com:8080", ProxyProtocol.HTTP, "domain.com", 8080, null, null,
        TestName = "Valid_Domain_With_Protocol")]
    [TestCase("domain.com:8080", ProxyProtocol.HTTP, "domain.com", 8080, null, null,
        TestName = "Valid_Domain_No_UserPass")]
    [TestCase("socks5://domain.com:8080", ProxyProtocol.SOCKS5, "domain.com", 8080, null, null,
        TestName = "Valid_SOCKS5")]
    [TestCase("http://38.225.18.182.suborbit.al:8080", ProxyProtocol.HTTP, "38.225.18.182.suborbit.al", 8080, null,
        null,
        TestName = "Valid_SubdomainDomain")]
    public void Colon_TryParse_Should_Parse_Valid_Proxies(
        string proxyString,
        ProxyProtocol expectedProtocol,
        string? expectedAddress,
        int expectedPort,
        string? expectedUser,
        string? expectedPass)
    {
        // Act
        var result = Colon.TryParse(proxyString, out var proxyData);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Is.True);
            Assert.That(proxyData, Is.Not.Null);
            Assert.That(proxyData!.Protocol, Is.EqualTo(expectedProtocol));
            Assert.That(proxyData!.Address, Is.EqualTo(expectedAddress));
            Assert.That(proxyData.Port, Is.EqualTo(expectedPort));
            Assert.That(proxyData.Username, Is.EqualTo(expectedUser));
            Assert.That(proxyData.Password, Is.EqualTo(expectedPass));
        });
    }


    [TestCase("", TestName = "Invalid_Empty")]
    [TestCase("d123d", TestName = "Invalid_Bad_string")]
    [TestCase("ftp://127.0.0.1:8080", TestName = "Invalid_Protocol")]
    //[TestCase("ftp://user:pass@domain.com:8080", TestName = "Invalid_Protocol_With_UserPass")]
    [TestCase("http://:8080", TestName = "Invalid_Missing_Address")]
    [TestCase("http://8080", TestName = "Invalid_No_Address")]
    [TestCase("http://127.0.0.1:", TestName = "Invalid_Missing_Port")]
    [TestCase("http://127.0.0.1", TestName = "Invalid_No_Port")]
    [TestCase("http://127.0.0.1:user", TestName = "Invalid_User_Without_Password")]
    [TestCase("http://[2001:db8::1]:8080", TestName = "Invalid_IPv6")]
    [TestCase("http://domain@:8080", TestName = "Invalid_Domain_With_Extra_At")]
    public void Colon_TryParse_Should_Return_False_For_Invalid_Proxies(string proxyString)
    {
        // Act
        var result = Colon.TryParse(proxyString, out var proxyData);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(proxyData, Is.Null);
    }


    [TestCase("socks4://user:pass@domain.com:8080", ProxyProtocol.SOCKS4, "domain.com", 8080, "user", "pass",
        TestName = "Valid_SOCKS4")]
    [TestCase("http://user:pass@127.0.0.1:8080", ProxyProtocol.HTTP, "127.0.0.1", 8080, "user", "pass",
        TestName = "Valid_UserPass_Before_IP")]
    [TestCase("http://user:pass@domain.com:8080", ProxyProtocol.HTTP, "domain.com", 8080, "user", "pass",
        TestName = "Valid_UserPass_With_Domain")]
    [TestCase("http://user:password@38.225.18.182.suborbit.al:8080", ProxyProtocol.HTTP, "38.225.18.182.suborbit.al",
        8080, "user", "password",
        TestName = "Valid_SubdomainDomain")]
    public void SignAt_TryParse_Should_Parse_Valid_Proxies(
        string proxyString,
        ProxyProtocol expectedProtocol,
        string? expectedAddress,
        int expectedPort,
        string? expectedUser,
        string? expectedPass)
    {
        // Act
        var result = SignAt.TryParse(proxyString, out var proxyData);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Is.True);
            Assert.That(proxyData, Is.Not.Null);
            Assert.That(proxyData!.Protocol, Is.EqualTo(expectedProtocol));
            Assert.That(proxyData!.Address, Is.EqualTo(expectedAddress));
            Assert.That(proxyData.Port, Is.EqualTo(expectedPort));
            Assert.That(proxyData.Username, Is.EqualTo(expectedUser));
            Assert.That(proxyData.Password, Is.EqualTo(expectedPass));
        });
    }

    [TestCase("", TestName = "Invalid_Empty")]
    [TestCase("d123d", TestName = "Invalid_Bad_string")]
    //[TestCase("ftp://127.0.0.1:8080", TestName = "Invalid_Protocol")]
    //[TestCase("ftp://user:pass@domain.com:8080", TestName = "Invalid_Protocol_With_UserPass")]
    [TestCase("http://:8080", TestName = "Invalid_Missing_Address")]
    [TestCase("http://8080", TestName = "Invalid_No_Address")]
    [TestCase("http://127.0.0.1:", TestName = "Invalid_Missing_Port")]
    [TestCase("http://127.0.0.1", TestName = "Invalid_No_Port")]
    [TestCase("http://127.0.0.1:user", TestName = "Invalid_User_Without_Password")]
    [TestCase("http://[2001:db8::1]:8080", TestName = "Invalid_IPv6")]
    [TestCase("http://domain@:8080", TestName = "Invalid_Domain_With_Extra_At")]
    [TestCase("http://domain.com:8080:user:pass", TestName = "Colon")]
    public void SignAt_TryParse_Should_Return_False_For_Invalid_Proxies(string proxyString)
    {
        // Act
        var result = SignAt.TryParse(proxyString, out var proxyData);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Is.False);
            Assert.That(proxyData, Is.Null);
        });
    }
}