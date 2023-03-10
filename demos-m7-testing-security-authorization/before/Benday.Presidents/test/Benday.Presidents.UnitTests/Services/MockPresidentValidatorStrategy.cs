using Benday.Presidents.Api.Models;

namespace Benday.Presidents.UnitTests.Services;

public class MockPresidentValidatorStrategy : IValidatorStrategy<President>
{
    public MockPresidentValidatorStrategy()
    {
        IsValidReturnValue = true;
    }

    public bool IsValidReturnValue { get; set; }

    public bool IsValid(President validateThis)
    {
        return IsValidReturnValue;
    }
}
