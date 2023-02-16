using Benday.Presidents.Api.Models;

namespace Benday.Presidents.Api.DataAccess;
public class LogEntry : Int32Identity
{
    public string FeatureName { get; set; }
    public DateTime LogDate { get; set; }
    public string LogType { get; set; }
    public string RequestIpAddress { get; set; }
    public string RequestUrl { get; set; }
    public string ReferrerUrl { get; set; }
    public string UserAgent { get; set; }
    public string Username { get; set; }
    public string Message { get; set; }
}