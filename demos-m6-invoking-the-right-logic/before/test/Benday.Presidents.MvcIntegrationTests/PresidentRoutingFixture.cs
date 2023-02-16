using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.Presidents.MvcIntegrationTests;

[TestClass]
public class PresidentRoutingFixture
{
    private WebApplicationFactory<Program> _SystemUnderTest;
    internal WebApplicationFactory<Program> SystemUnderTest
    {
        get
        {
            if (_SystemUnderTest == null)
            {
                _SystemUnderTest =
                    new WebApplicationFactory<Program>();
            }

            return _SystemUnderTest;
        }
    }

    private async Task PopulateTestData()
    {
        var client = SystemUnderTest.CreateDefaultClient();

        var response = await client.GetAsync("/president/VerifyDatabaseIsPopulated");

        Assert.IsNotNull(response, "Response was null.");

        int statusCodeAsInt = (int)response.StatusCode;

        Assert.IsTrue(statusCodeAsInt < 400,
            "Got an error response from populating test data.");
    }
}
