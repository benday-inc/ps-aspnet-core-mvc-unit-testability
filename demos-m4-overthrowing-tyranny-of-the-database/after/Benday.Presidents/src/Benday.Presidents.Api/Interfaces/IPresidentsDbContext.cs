using Benday.Presidents.Api.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Benday.Presidents.Common;

public interface IPresidentsDbContext
{
    DbSet<Person> Persons { get; set; }
    DbSet<PersonFact> PersonFacts { get; set; }
    int SaveChanges();
}