using Benday.DataAccess;

namespace Benday.Presidents.Api.DataAccess;

public interface IFeatureRepository : IRepository<Feature>
{
    IList<Feature> GetByUsername(string username);
}
