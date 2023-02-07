using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.Services;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api;
using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.UnitTests.Services
{
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

        [TestMethod]
        public void AdaptPersonToPresident_ConsecutiveTerm()
        {
            // arrange
            var fromValue = UnitTestUtility.GetThomasJeffersonAsPerson();
            var toValue = new President();

            // act
            SystemUnderTest.Adapt(fromValue, toValue);

            // assert
            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void AdaptPersonToPresident_TwoNonConsecutiveTerms()
        {
            // arrange
            var fromValue = UnitTestUtility.GetGroverClevelandAsPerson();
            var toValue = new President();

            // act
            SystemUnderTest.Adapt(fromValue, toValue);

            // assert
            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void AdaptPresidentToPerson_ConsecutiveTerm()
        {
            // arrange
            var fromValue = UnitTestUtility.GetThomasJeffersonAsPresident();
            var toValue = new Person();

            // act
            SystemUnderTest.Adapt(fromValue, toValue);

            // assert
            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void AdaptPresidentToPerson_TwoNonConsecutiveTerms()
        {
            // arrange
            var fromValue = UnitTestUtility.GetGroverClevelandAsPresident();
            var toValue = new Person();

            // act
            SystemUnderTest.Adapt(fromValue, toValue);

            // assert
            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        
    }
}
