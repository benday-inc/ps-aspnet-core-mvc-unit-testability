
namespace Benday.Presidents.Api.Services;

public interface ILogger
{
    void LogFeatureUsage(string featureName);
    void LogCustomerSatisfaction(string feedback);
}
