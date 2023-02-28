namespace Benday.Presidents.WebUI.Controllers;

public interface ITestDataUtility
{
    Task CreatePresidentTestData();
    Task VerifyDatabaseIsPopulated();
}

