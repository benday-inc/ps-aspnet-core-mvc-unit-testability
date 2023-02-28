
namespace Benday.Presidents.Api.Interfaces;

public interface IFeatureManager
{
    bool Search { get; }

    bool SearchByBirthDeathState { get; }

    bool FeatureUsageLogging { get; }

    bool CustomerSatisfaction { get; }
}
