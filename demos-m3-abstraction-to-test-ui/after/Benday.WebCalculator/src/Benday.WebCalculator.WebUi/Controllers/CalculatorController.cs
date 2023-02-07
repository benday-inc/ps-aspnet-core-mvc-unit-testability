using System;
using System.Collections.Generic;
using System.Linq;
using Benday.WebCalculator.Api;
using Benday.WebCalculator.WebUi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Benday.WebCalculator.WebUi.Controllers
{
    public class CalculatorController : Controller
    {
        private ICalculatorService _CalculatorService;

        public CalculatorController(ICalculatorService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service", "Argument cannot be null.");
            }

            _CalculatorService = service;
        }

        public IActionResult Index()
        {
            var model = new CalculatorViewModel();

            model.Operator = CalculatorConstants.Message_ChooseAnOperator;

            model.Operators = GetOperators();

            model.Message = String.Empty;

            model.IsResultValid = false;

            return View(model);
        }

        private List<SelectListItem> GetOperators()
        {
            var operators = new List<SelectListItem>();

            operators.Add(
                String.Empty,
                CalculatorConstants.Message_ChooseAnOperator,
                true);

            operators.Add(CalculatorConstants.OperatorAdd);
            operators.Add(CalculatorConstants.OperatorSubtract);
            operators.Add(CalculatorConstants.OperatorMultiply);
            operators.Add(CalculatorConstants.OperatorDivide);

            return operators;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate(CalculatorViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "Argument cannot be null.");
            }

            var operation = model.Operator;

            if (operation == CalculatorConstants.OperatorAdd)
            {
                // perform add
                model.ResultValue = _CalculatorService.Add(
                    model.Value1, model.Value2
                );

                model.IsResultValid = true;
                model.Message = CalculatorConstants.Message_Success;
                PopulateOperators(model, operation);

                return View("Index", model);
            }
            else if (operation == CalculatorConstants.OperatorSubtract)
            {
                model.ResultValue =
                    _CalculatorService.Subtract(
                        model.Value1, model.Value2);
                model.Message = CalculatorConstants.Message_Success;
                model.IsResultValid = true;
                PopulateOperators(model, operation);

                return View("Index", model);
            }
            else if (operation == CalculatorConstants.OperatorMultiply)
            {
                model.ResultValue =
                    _CalculatorService.Multiply(
                        model.Value1, model.Value2);
                model.Message = CalculatorConstants.Message_Success;
                model.IsResultValid = true;
                PopulateOperators(model, operation);

                return View("Index", model);
            }
            else if (operation == CalculatorConstants.OperatorDivide)
            {
                if (model.Value2 == 0)
                {
                    model.ResultValue = 0;
                    model.IsResultValid = false;
                    model.Message = CalculatorConstants.Message_CantDivideByZero;
                    PopulateOperators(model, operation);
                }
                else
                {
                    model.ResultValue =
                        _CalculatorService.Divide(
                            model.Value1, model.Value2);
                    model.Message = CalculatorConstants.Message_Success;
                    model.IsResultValid = true;
                    PopulateOperators(model, operation);
                }

                return View("Index", model);
            }
            else
            {
                return BadRequest();
            }
        }

        private void PopulateOperators(CalculatorViewModel model, string operation)
        {
            model.Operator = operation;

            var operators = GetOperators();

            foreach (var item in operators)
            {
                item.Selected = false;
            }

            var selectThisOperator = (from temp in operators
                                      where temp.Text == operation
                                      select temp).FirstOrDefault();

            if (selectThisOperator != null)
            {
                selectThisOperator.Selected = true;
            }

            model.Operators = operators;
        }
    }
}