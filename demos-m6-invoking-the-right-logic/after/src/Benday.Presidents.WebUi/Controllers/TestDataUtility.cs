using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Services;
using System.Xml.Linq;
using Benday.Presidents.Api;
using Benday.Presidents.WebUi.TestData;
using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.WebUI.Controllers;

public class TestDataUtility : ITestDataUtility
{
    private IPresidentService _Service;
    private PresidentsDbContext _DbContext;

    public TestDataUtility(IPresidentService service, PresidentsDbContext dbContext)
    {
        if (service == null)
            throw new ArgumentNullException("service", "service is null.");

        _Service = service;

        if (dbContext == null)
        {
            throw new ArgumentNullException("dbContext", "Argument cannot be null.");
        }

        _DbContext = dbContext;
    }

    public void CreatePresidentTestData()
    {
        var xml = TestDataResource.UsPresidentsXml;

        List<President> allPresidents = PopulatePresidentsFromXml(xml);

        DeleteAll();

        allPresidents.ForEach(x => _Service.Save(x));
    }

    public void VerifyDatabaseIsPopulated()
    {
        _DbContext.Database.EnsureCreated();

        var presidents = _Service.GetPresidents();

        if (presidents == null || presidents.Count == 0)
        {
            CreatePresidentTestData();
        }
    }

    private List<President> PopulatePresidentsFromXml(string xml)
    {
        var returnValue = new List<President>();

        var root = XElement.Parse(xml);

        var presidents = root.ElementsByLocalName("president");

        President groverCleveland = null;

        foreach (var fromElement in presidents)
        {
            var currentPresident = GetPresidentFromXml(fromElement);

            if (currentPresident.LastName == "Cleveland")
            {
                // grover cleveland had two non-consecutive terms
                // only create one record for grover 
                // with two terms
                if (groverCleveland == null)
                {
                    groverCleveland = currentPresident;
                    returnValue.Add(currentPresident);
                }
                else
                {
                    groverCleveland.Terms.Add(currentPresident.Terms[0]);
                }
            }
            else
            {
                returnValue.Add(currentPresident);
            }
        }

        return returnValue;
    }

    private President GetPresidentFromXml(XElement fromValue)
    {
        President toValue = new President();

        toValue.BirthCity = fromValue.AttributeValue("birthcity");
        toValue.BirthState = fromValue.AttributeValue("birthstate");
        toValue.BirthDate = SafeToDateTime(fromValue.AttributeValue("birthdate"));

        toValue.DeathCity = fromValue.AttributeValue("deathcity");
        toValue.DeathState = fromValue.AttributeValue("deathstate");
        toValue.DeathDate = SafeToDateTime(fromValue.AttributeValue("deathdate"));

        toValue.FirstName = fromValue.AttributeValue("firstname");
        toValue.LastName = fromValue.AttributeValue("lastname");

        toValue.ImageFilename = fromValue.AttributeValue("image-filename");

        toValue.AddTerm(
            "President",
            SafeToDateTime(fromValue.AttributeValue("start")),
            SafeToDateTime(fromValue.AttributeValue("end")),
            SafeToInt32(fromValue.AttributeValue("id"))
            );

        return toValue;
    }

    private DateTime SafeToDateTime(string fromValue)
    {
        DateTime temp;

        if (DateTime.TryParse(fromValue, out temp) == true)
        {
            return temp;
        }
        else
        {
            return default(DateTime);
        }
    }

    private int SafeToInt32(string fromValue)
    {
        int temp;

        if (Int32.TryParse(fromValue, out temp) == true)
        {
            return temp;
        }
        else
        {
            return default(int);
        }
    }

    private void DeleteAll()
    {
        var allPresidents = _Service.GetPresidents();

        foreach (var item in allPresidents)
        {
            _Service.DeletePresidentById(item.Id);
        }
    }
}

