﻿using Benday.Presidents.Api;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Benday.Presidents.WebUI.Controllers;

public class PresidentController : Controller
{
    private const int ID_FOR_CREATE_NEW_PRESIDENT = 0;
    private IPresidentService _Service;
    private IValidatorStrategy<President> _Validator;
    private ITestDataUtility _TestDataUtility;

    public PresidentController(IPresidentService service,
        IValidatorStrategy<President> validator,
        ITestDataUtility testDataUtility
        )
    {
        if (service == null)
            throw new ArgumentNullException("service", "service is null.");

        if (validator == null)
        {
            throw new ArgumentNullException("validator", "Argument cannot be null.");
        }

        _Validator = validator;
        _Service = service;
        _TestDataUtility = testDataUtility;
    }

    public ActionResult Index()
    {
        var presidents = _Service.GetPresidents();

        return View(presidents);
    }

    public ActionResult Details(int? id)
    {
        if (id == null || id.HasValue == false)
        {
            return new BadRequestResult();
        }

        var president = _Service.GetPresidentById(id.Value);

        if (president == null)
        {
            return NotFound();
        }

        return View(president);
    }

    public ActionResult Create()
    {
        return RedirectToAction("Edit", new { id = ID_FOR_CREATE_NEW_PRESIDENT });
    }

    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return new BadRequestResult();
        }

        President president;

        if (id.Value == ID_FOR_CREATE_NEW_PRESIDENT)
        {
            // create new
            president = new President();
            president.AddTerm(PresidentsConstants.President,
                default(DateTime),
                default(DateTime), 0);
        }
        else
        {
            president = _Service.GetPresidentById(id.Value);
        }

        if (president == null)
        {
            return NotFound();
        }

        return View(president);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(President president)
    {
        if (_Validator.IsValid(president) == true)
        {
            bool isCreateNew = false;

            if (president.Id == ID_FOR_CREATE_NEW_PRESIDENT)
            {
                isCreateNew = true;
            }
            else
            {
                President toValue =
                    _Service.GetPresidentById(president.Id);

                if (toValue == null)
                {
                    return new BadRequestObjectResult(
                        String.Format("Unknown president id '{0}'.", president.Id));
                }
            }

            _Service.Save(president);

            if (isCreateNew == true)
            {
                RedirectToAction("Edit", new { id = president.Id });
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        return View(president);
    }

    public ActionResult ResetDatabase()
    {
        _TestDataUtility.CreatePresidentTestData();

        return RedirectToAction("Index");
    }

    public ActionResult VerifyDatabaseIsPopulated()
    {
        _TestDataUtility.VerifyDatabaseIsPopulated();

        return RedirectToAction("Index");
    }
}