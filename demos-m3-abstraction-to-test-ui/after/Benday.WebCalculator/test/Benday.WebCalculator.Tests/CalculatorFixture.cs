using Benday.WebCalculator.Api;

namespace Benday.WebCalculator.Tests;

[TestClass]
public class CalculatorFixture
{
    [TestInitialize]
    public void OnTestInitialize()
    {
        _SystemUnderTest = null;
    }

    private Calculator? _SystemUnderTest;
    public Calculator? SystemUnderTest
    {
        get
        {
            if (_SystemUnderTest == null)
            {
                _SystemUnderTest = new Calculator();
            }

            return _SystemUnderTest;
        }
    }

    [TestMethod]
    public void Add()
    {
        // arrange
        double value1 = 2;
        double value2 = 3;
        double expected = 5;

        // act
        double actual = SystemUnderTest.Add(
            value1, value2);

        // assert
        Assert.AreEqual<double>(expected, actual, "Wrong result.");
    }

    [TestMethod]
    public void Subtract()
    {
        // arrange
        double value1 = 6;
        double value2 = 2;
        double expected = 4;

        // act
        double actual = SystemUnderTest.Subtract(
            value1, value2);

        // assert
        Assert.AreEqual<double>(expected, actual, "Wrong result.");
    }

    [TestMethod]
    public void Multiply()
    {
        // arrange
        double value1 = 6;
        double value2 = 2;
        double expected = 12;

        // act
        double actual = SystemUnderTest.Multiply(
            value1, value2);

        // assert
        Assert.AreEqual<double>(expected, actual, "Wrong result.");
    }

    [TestMethod]
    public void Divide()
    {
        // arrange
        double value1 = 6;
        double value2 = 2;
        double expected = 3;

        // act
        double actual = SystemUnderTest.Divide(
            value1, value2);

        // assert
        Assert.AreEqual<double>(expected, actual, "Wrong result.");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DivideByZeroThrowsException()
    {
        // arrange
        double value1 = 6;
        double value2 = 0;

        // act
        double actual = SystemUnderTest.Divide(
            value1, value2);
    }
}