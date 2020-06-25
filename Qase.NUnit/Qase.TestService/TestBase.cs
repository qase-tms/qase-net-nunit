using NUnit.Framework;
using OpenQA.Selenium;
using Qase.API.Qase;
using Qase.API.Qase.Model.TestRunResults;
using System;
using System.Collections.Generic;

namespace Qase.TestService
{
  public class TestBase
  {
    private TestContext _fixtureContext;
    private QaseAPI  _qaseAPI;
    //private string _suiteid, _projectid;
    //private bool IgnoreAddResults = false;
    //private int _projectIdInt, _suiteIdInt, _caseId;
    private List<TestRunResult> _resultsForCases;
    public IWebDriver Driver { get; set; }

    [OneTimeSetUp]
    public void FixtureSetup()
    {
      try
      {
        _fixtureContext = TestContext.CurrentContext;

        InitQase();
        //ValidateSuiteIdAndProjectId();
        _resultsForCases = new List<TestRunResult>();
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
          var runId = _qaseAPI.CreateNewTestRunAsync(new Run { project_id = _projectIdInt, suite_id = _suiteIdInt, include_all = false });
          if (runId > 0) _qaseAPI.AddResultsForCases(runId, _resultsForCases);
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
          var result = new Result { case_id = _caseId, comment = TestContext.CurrentContext.Result.Message };
          var resultState = TestContext.CurrentContext.Result.Outcome;

          if (resultState == ResultState.Success) result.status_id = 1;
          else if (resultState == ResultState.Inconclusive) result.status_id = 4;
          else result.status_id = 5;

          _resultsForCases.Add(result);
        }
      }
    }

    private void InitQase()
    {
      //var testrailurl = ConfigurationManager.AppSettings["testrailurl"];
      //var username = ConfigurationManager.AppSettings["username"];
      //var password = ConfigurationManager.AppSettings["password"];

      //if (string.IsNullOrEmpty(testrailurl)) throw new Exception("Invalid testrail url");
      //if (string.IsNullOrEmpty(username)) throw new Exception("Invalid testrail username");
      //if (string.IsNullOrEmpty(password)) throw new Exception("Invalid testrail password");

      _qaseAPI = new QaseAPI(url, api_token);
    }

    //private void ValidateSuiteIdAndProjectId()
    //{
    //  _suiteid = _fixtureContext.Test.Properties.Get("suiteid")?.ToString();
    //  _projectid = _fixtureContext.Test.Properties.Get("projectid")?.ToString();

    //  if (string.IsNullOrEmpty(_suiteid)) throw new Exception("Invalid suite id");
    //  if (string.IsNullOrEmpty(_projectid)) throw new Exception("Invalid project id");

    //  if (!Int32.TryParse(_projectid, out _projectIdInt)) throw new Exception("Project id not valid int");
    //  if (!Int32.TryParse(_suiteid, out _suiteIdInt)) throw new Exception("Suite id not valid int");

    //  // we should add validation for project and suite id

    //}
  }

}