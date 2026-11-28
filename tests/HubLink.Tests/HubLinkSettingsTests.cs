using HubLink.Application.Configuration;
using HubLink.Infrastructure.Configuration;

namespace HubLink.Tests;

public class HubLinkSettingsTests
{
    [Fact]
    public void BuildShipmentsUri_UsesBaseUrlPathAndBranch()
    {
        var s = new HubLinkSettings { BaseUrl = "http://127.0.0.1:5080", ShipmentsPath = "/api/shipments", BranchCode = "BR-001" };

        Assert.Equal("http://127.0.0.1:5080/api/shipments?branchCode=BR-001", s.BuildShipmentsUri().ToString());
    }

    [Fact]
    public void BuildShipmentsUri_AppendsExtraQuery_ForTestModes()
    {
        var s = new HubLinkSettings { ShipmentsPath = "/api/test/shipments", ExtraQuery = "mode=delay" };

        Assert.Equal("http://127.0.0.1:5080/api/test/shipments?branchCode=BR-001&mode=delay", s.BuildShipmentsUri().ToString());
    }

    [Fact]
    public void BuildShipmentsUri_RejectsInvalidBaseUrl()
    {
        var s = new HubLinkSettings { BaseUrl = "not a url" };

        Assert.Throws<InvalidOperationException>(() => s.BuildShipmentsUri());
    }

    [Fact]
    public void Loader_ReadsValuesFromAppsettingsFile()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            File.WriteAllText(Path.Combine(dir.FullName, "appsettings.json"),
                """{ "HubLink": { "BaseUrl": "http://127.0.0.1:5081", "BranchCode": "BR-002", "TimeoutSeconds": 5 } }""");

            var s = SettingsLoader.Load(dir.FullName);

            Assert.Equal("http://127.0.0.1:5081", s.BaseUrl);
            Assert.Equal("BR-002", s.BranchCode);
            Assert.Equal(5, s.TimeoutSeconds);
        }
        finally
        {
            dir.Delete(true);
        }
    }
}
