using Benday.Presidents.Api.Models;

namespace Benday.Presidents.Api.DataAccess;

public class Subscription : Int32Identity
{
    public string Username { get; set; }
    public string SubscriptionLevel { get; set; }
}
