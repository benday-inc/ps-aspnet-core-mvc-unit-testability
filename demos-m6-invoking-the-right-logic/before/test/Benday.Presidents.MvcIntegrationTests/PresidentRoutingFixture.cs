using Benday.Presidents.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Benday.Presidents.MvcIntegrationTests
{
    [TestClass]
    public class PresidentRoutingFixture
    {
        private WebApplicationFactory<Benday.Presidents.WebUi.Startup> _SystemUnderTest;
        public WebApplicationFactory<Benday.Presidents.WebUi.Startup> SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest =
                        new WebApplicationFactory<Benday.Presidents.WebUi.Startup>();
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
}
