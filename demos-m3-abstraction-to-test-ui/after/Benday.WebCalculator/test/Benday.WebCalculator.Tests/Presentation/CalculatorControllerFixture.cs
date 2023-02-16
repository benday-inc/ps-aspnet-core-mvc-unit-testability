using System;
using System.Collections.Generic;
using System.Linq;
using Benday.WebCalculator.Api;
using Benday.WebCalculator.WebUi;
using Benday.WebCalculator.WebUi.Controllers;
using Benday.WebCalculator.WebUi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.WebCalculator.Tests.Presentation;

[TestClass]
public class CalculatorControllerFixture
{
    private CalculatorController _SystemUnderTest;
    public CalculatorController SystemUnderTest
    {
        get
        {
            if (_SystemUnderTest == null)
            {
                _SystemUnderTest =
                    new CalculatorController(
                        CalculatorServiceInstance
                    );
            }

            return _SystemUnderTest;
        }
    }

    private ICalculatorService _CalculatorServiceInstance;

    public ICalculatorService CalculatorServiceInstance
    {
        get
        {
            if (_CalculatorServiceInstance == null)
            {
                _CalculatorServiceInstance =
                    new Calculator();
            }

            return _CalculatorServiceInstance;
        }
    }

    [TestMethod]
    public void CalculatorController_Index_ModelIsNotNull()
    {
        var actual =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        Assert.IsNotNull(actual, "Model was null.");
    }

    [TestMethod]
    public void CalculatorController_Index_Model_Value1IsInitialized()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        var actual = model.Value1;

        var expected = 0d;

        Assert.AreEqual<double>(expected, actual, "Value1 field value was wrong.");
    }


    [TestMethod]
    public void CalculatorController_Index_Model_Value2IsInitialized()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        var actual = model.Value2;

        var expected = 0d;

        Assert.AreEqual<double>(expected, actual, "Value2 field value was wrong.");
    }

    [TestMethod]
    public void CalculatorController_Index_Model_OperatorIsInitialized()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        var actual = model.Operator;

        var expected = CalculatorConstants.Message_ChooseAnOperator;

        Assert.AreEqual<string>(expected, actual, "Operator field value was wrong.");
    }

    [TestMethod]
    public void CalculatorController_Index_Model_MessageIsInitialized()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        var actual = model.Message;

        var expected = String.Empty;

        Assert.AreEqual<string>(expected, actual, "Message field value was wrong.");
    }

    [TestMethod]
    public void CalculatorController_Index_Model_IsResultValidShouldBeFalse()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        var actual = model.IsResultValid;

        var expected = false;

        Assert.AreEqual<bool>(expected, actual, "IsResultValid field value was wrong.");
    }

    [TestMethod]
    public void CalculatorController_Index_Model_OperatorsCollectionIsPopulated()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        Assert.IsNotNull(model.Operators,
            "Operators collection was null.");

        var actual = model.Operators.Select(x => x.Text).ToList();

        var expected = new List<string>();

        expected.Add(CalculatorConstants.Message_ChooseAnOperator);
        expected.Add(CalculatorConstants.OperatorAdd);
        expected.Add(CalculatorConstants.OperatorSubtract);
        expected.Add(CalculatorConstants.OperatorMultiply);
        expected.Add(CalculatorConstants.OperatorDivide);

        CollectionAssert.AreEqual(expected, actual,
            "Wrong values in operators collection.");
    }

    [TestMethod]
    public void CalculatorController_Calculate_Add()
    {
        // arrange
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        double value1 = 2;
        double value2 = 3;
        double expected = 5;

        model.Value1 = value1;
        model.Value2 = value2;
        model.Operator = CalculatorConstants.OperatorAdd;

        // act
        var actual =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Calculate(model));


        // assert
        Assert.IsTrue(actual.IsResultValid, "Result should be valid.");
        Assert.AreEqual<double>(expected, actual.ResultValue, "Result was wrong.");
        Assert.AreEqual<string>(CalculatorConstants.Message_Success,
            actual.Message, "Message was wrong.");

        AssertOperatorsAndSelectedOperator(model,
            CalculatorConstants.OperatorAdd);
    }

    [TestMethod]
    public void CalculatorController_Calculate_Subtract()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        double value1 = 2;
        double value2 = 3;
        double expected = -1;

        model.Value1 = value1;
        model.Value2 = value2;
        model.Operator = CalculatorConstants.OperatorSubtract;

        var actual =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Calculate(model));

        Assert.IsTrue(actual.IsResultValid, "Result should be valid.");
        Assert.AreEqual<double>(expected, actual.ResultValue, "Result was wrong.");
        Assert.AreEqual<string>(CalculatorConstants.Message_Success,
            actual.Message, "Message was wrong.");

        AssertOperatorsAndSelectedOperator(model,
            CalculatorConstants.OperatorSubtract);
    }

    [TestMethod]
    public void CalculatorController_Calculate_Multiply()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        double value1 = 2;
        double value2 = 3;
        double expected = 6;

        model.Value1 = value1;
        model.Value2 = value2;
        model.Operator = CalculatorConstants.OperatorMultiply;

        var actual =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Calculate(model));

        Assert.IsTrue(actual.IsResultValid, "Result should be valid.");
        Assert.AreEqual<double>(expected, actual.ResultValue, "Result was wrong.");
        Assert.AreEqual<string>(CalculatorConstants.Message_Success,
            actual.Message, "Message was wrong.");

        AssertOperatorsAndSelectedOperator(model,
            CalculatorConstants.OperatorMultiply);
    }

    [TestMethod]
    public void CalculatorController_Calculate_Divide()
    {
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        double value1 = 8;
        double value2 = 4;
        double expected = 2;

        model.Value1 = value1;
        model.Value2 = value2;
        model.Operator = CalculatorConstants.OperatorDivide;

        var actual =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Calculate(model));

        Assert.IsTrue(actual.IsResultValid, "Result should be valid.");
        Assert.AreEqual<double>(expected, actual.ResultValue, "Result was wrong.");
        Assert.AreEqual<string>(CalculatorConstants.Message_Success,
            actual.Message, "Message was wrong.");

        AssertOperatorsAndSelectedOperator(model,
            CalculatorConstants.OperatorDivide);
    }

    [TestMethod]
    public void CalculatorController_Calculate_DivideByZero()
    {
        // arrange
        var model =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Index());

        double value1 = 8;
        double value2 = 0;

        model.Value1 = value1;
        model.Value2 = value2;
        model.Operator = CalculatorConstants.OperatorDivide;

        // act
        var actual =
            UnitTestUtility.GetModel<CalculatorViewModel>(
                SystemUnderTest.Calculate(model));

        // assert
        Assert.IsFalse(actual.IsResultValid, "Result should not be valid.");
        Assert.AreEqual<double>(0, actual.ResultValue, "Result was wrong.");
        Assert.AreEqual<string>(CalculatorConstants.Message_CantDivideByZero,
            actual.Message, "Message was wrong.");

        AssertOperatorsAndSelectedOperator(model,
            CalculatorConstants.OperatorDivide);
    }

    private void AssertOperatorsAndSelectedOperator(
        CalculatorViewModel model, string expectedSelectedOperator)
    {
        Assert.IsNotNull(model.Operators, "Operators collection was null.");

        var actual = model.Operators.Select(x => x.Text).ToList();

        var expected = new List<string>();

        expected.Add(CalculatorConstants.Message_ChooseAnOperator);
        expected.Add(CalculatorConstants.OperatorAdd);
        expected.Add(CalculatorConstants.OperatorSubtract);
        expected.Add(CalculatorConstants.OperatorMultiply);
        expected.Add(CalculatorConstants.OperatorDivide);

        CollectionAssert.AreEqual(expected, actual,
            "Operators in collection were wrong.");

        AssertSelectedOperator(model, expectedSelectedOperator);
    }

    private void AssertSelectedOperator(
        CalculatorViewModel model, string expectedSelectedOperator)
    {
        Assert.IsNotNull(model.Operators, "Operators collection was null.");

        Assert.AreEqual<string>(expectedSelectedOperator, model.Operator,
            "Operator property was wrong.");

        var match = (from temp in model.Operators
                     where temp.Text == expectedSelectedOperator
                     select temp).FirstOrDefault();

        Assert.IsNotNull(match,
            "Could not find '{0}' in Operators.",
            expectedSelectedOperator);

        Assert.IsTrue(match.Selected,
            "Operator '{0}' should have been selected.",
            expectedSelectedOperator);
    }

}
