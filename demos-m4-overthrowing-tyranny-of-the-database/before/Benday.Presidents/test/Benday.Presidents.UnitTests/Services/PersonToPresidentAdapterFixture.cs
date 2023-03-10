using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.Services;

namespace Benday.Presidents.UnitTests.Services;

[TestClass]
public class PersonToPresidentAdapterFixture
{
    [TestInitialize]
    public void OnTestInitialize()
    {
        _SystemUnderTest = null;
    }

    private PersonToPresidentAdapter _SystemUnderTest;
    public PersonToPresidentAdapter SystemUnderTest
    {
        get
        {
            if (_SystemUnderTest == null)
            {
                _SystemUnderTest = new PersonToPresidentAdapter();
            }

            return _SystemUnderTest;
        }
    }
}
