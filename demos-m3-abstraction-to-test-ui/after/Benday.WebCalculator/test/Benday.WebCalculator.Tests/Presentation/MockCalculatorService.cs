using Benday.WebCalculator.Api;

namespace Benday.WebCalculator.Tests.Presentation;

public class MockCalculatorService : ICalculatorService
{
    public MockCalculatorService()
    {
        AddWasCalled = false;
        SubtractWasCalled = false;
        MultiplyWasCalled = false;
        DivideWasCalled = false;
    }

    public bool AddWasCalled
    {
        get; private set;
    }

    public bool SubtractWasCalled
    {
        get; private set;
    }

    public bool MultiplyWasCalled
    {
        get; private set;
    }

    public bool DivideWasCalled
    {
        get; private set;
    }

    public double ReturnValue { get; set; }

    public double Add(double value1, double value2)
    {
        AddWasCalled = true;
        return ReturnValue;
    }

    public double Divide(double value1, double value2)
    {
        DivideWasCalled = true;
        return ReturnValue;
    }

    public double Multiply(double value1, double value2)
    {
        MultiplyWasCalled = true;
        return ReturnValue;
    }

    public double Subtract(double value1, double value2)
    {
        SubtractWasCalled = true;
        return ReturnValue;
    }
}
