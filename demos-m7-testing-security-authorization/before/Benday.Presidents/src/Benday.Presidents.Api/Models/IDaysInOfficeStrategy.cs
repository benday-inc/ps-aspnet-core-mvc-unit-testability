
namespace Benday.Presidents.Api.Models;

public interface IDaysInOfficeStrategy
{
    int GetDaysInOffice(IEnumerable<Term> terms);
}
