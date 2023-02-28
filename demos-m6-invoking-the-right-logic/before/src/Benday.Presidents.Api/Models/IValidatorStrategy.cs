
namespace Benday.Presidents.Api.Models;

public interface IValidatorStrategy<T>
{
    bool IsValid(T validateThis);
}
