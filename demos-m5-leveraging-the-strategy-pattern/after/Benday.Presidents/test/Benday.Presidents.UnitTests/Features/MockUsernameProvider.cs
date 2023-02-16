using Benday.Presidents.Api.Interfaces;

namespace Benday.Presidents.UnitTests.Features;

public class MockUsernameProvider : IUsernameProvider
{
    public string ReturnThisUsername { get; set; }

    public string GetUsername()
    {
        return ReturnThisUsername;
    }
}
