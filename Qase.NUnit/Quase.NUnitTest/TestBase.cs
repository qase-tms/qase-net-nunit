using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Qase.API.Qase;
using Qase.API.Qase.Model;
using Qase.API.Qase.Model.TestRunResults;
using Qase.API.Qase.Model.TestRuns;
using System;
using System.Collections.Generic;
using System.IO;

namespace Quase.NUnitTest
{
  public class TestBase
  {
    private TestContext _fixtureContext;
    private QaseAPI  _qaseAPI;
    public IConfiguration _configuration { get; set; }
    private string _code;
    private bool IgnoreAddResults = false;
    private int /*_projectIdInt, _suiteIdInt,*/ _caseId;
    private List<AddTestRunResultRequest> _resultsForCases;
    public IWebDriver Driver { get; set; }

    [OneTimeSetUp]
    public void FixtureSetup()
    {
      try
      {
        _fixtureContext = TestContext.CurrentContext;

        InitQase();
        ValidateProjectCode();
        _resultsForCases = new List<AddTestRunResultRequest>();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        IgnoreAddResults = true;
      }
    }

    [OneTimeTearDown]
    public void FixtureTearDown()
    {
      if (!IgnoreAddResults)
      {
        if (_resultsForCases.Count > 0)
        {
          var cases = new List<int>();
          cases.Add(_caseId);

          var runId = _qaseAPI.CreateNewTestRunAsync(_code, new CreateTestRunRequest 
          {
            Title = "Test.Net",
            Cases = cases
          }).Result
          .Result.Id;

          if (runId > 0)
          {
            foreach (var result in _resultsForCases)
            {
              var hash = _qaseAPI.AddTestRunResultAsync(_code, runId, result).Result.Result.Hash;
            }
          }
        }
      }
    }

    [TearDown]
    public void Cleanup()
    {
      if (!IgnoreAddResults)
      {
        var _caseid = 0;
        var caseid = TestContext.CurrentContext.Test.Properties.Get("caseid")?.ToString();
        if (Int32.TryParse(caseid, out _caseId))
        {
          var result = new AddTestRunResultRequest { CaseId = _caseId,  Comment = TestContext.CurrentContext.Result.Message };
          var resultState = TestContext.CurrentContext.Result.Outcome;

          if (resultState == ResultState.Success) result.Status = StatusTestRunResult.passed.ToString();
          else if (resultState == ResultState.Inconclusive) result.Status = StatusTestRunResult.blocked.ToString();
          else result.Status = StatusTestRunResult.failed.ToString();

          _resultsForCases.Add(result);
        }
      }
    }

    private void InitQase()
    {
      var configurationBuilder = new ConfigurationBuilder();
      var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
      _configuration = configurationBuilder.AddJsonFile(path, false).Build();

      var urlAPI = _configuration["UrlAPI"];
      var apiToken = _configuration["Api_Token"];

      if (string.IsNullOrEmpty(urlAPI)) throw new Exception("Invalid qase url");
      if (string.IsNullOrEmpty(apiToken)) throw new Exception("Invalid qase api token");

      _qaseAPI = new QaseAPI(urlAPI, apiToken);
    }

    private void ValidateProjectCode()
    {
      _code = _fixtureContext.Test.Properties.Get("code")?.ToString();

      if (string.IsNullOrEmpty(_code)) throw new Exception("Invalid project code");
    }
  }

}