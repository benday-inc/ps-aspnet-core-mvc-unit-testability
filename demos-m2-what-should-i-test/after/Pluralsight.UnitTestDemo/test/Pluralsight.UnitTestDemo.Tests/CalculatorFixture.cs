using Pluralsight.UnitTestDemo.Api;

namespace Pluralsight.UnitTestDemo.Tests;

[TestClass]
public class CalculatorFixture
{
    [TestInitialize]
    public void OnTestInitialize()
    {
        _SystemUnderTest = null;
    }

    private Calculator _SystemUnderTest;
    public Calculator SystemUnderTest
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
        int value1 = 2;
        int value2 = 3;
        int expected = 5;

        // act
        int actual = SystemUnderTest.Add(
            value1, value2);

        // assert
        Assert.AreEqual<int>(expected, actual, "Wrong result.");
    }

    [TestMethod]
    public void Subtract()
    {
        // arrange
        int value1 = 6;
        int value2 = 2;
        int expected = 4;

        // act
        int actual = SystemUnderTest.Subtract(
            value1, value2);

        // assert
        Assert.AreEqual<int>(expected, actual, "Wrong result.");
    }

    [TestMethod]
    public void Multiply()
    {
        // arrange
        int value1 = 6;
        int value2 = 2;
        int expected = 12;

        // act
        int actual = SystemUnderTest.Multiply(
            value1, value2);

        // assert
        Assert.AreEqual<int>(expected, actual, "Wrong result.");
    }

    [TestMethod]
    public void Divide()
    {
        // arrange
        int value1 = 6;
        int value2 = 2;
        int expected = 3;

        // act
        int actual = SystemUnderTest.Divide(
            value1, value2);

        // assert
        Assert.AreEqual<int>(expected, actual, "Wrong result.");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DivideByZeroThrowsException()
    {
        // arrange
        int value1 = 6;
        int value2 = 0;

        // act
        int actual = SystemUnderTest.Divide(
            value1, value2);
    }
}