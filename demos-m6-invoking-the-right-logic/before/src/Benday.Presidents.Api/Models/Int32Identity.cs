using Benday.DataAccess;

namespace Benday.Presidents.Api.Models;

public abstract class Int32Identity : IInt32Identity
{
    public int Id { get; set; }
}
