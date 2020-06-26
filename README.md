# qase-net-nunit
# [Qase TMS](https://qase.io) NUnit

## Configuration

appsettings.json configuration:

```json
{
  "UrlAPI": "https://api.qase.io/v1",
  "Api_Token": "your api token"
}
```

## Initialization Qase

```C#
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
```


## Result test

```C#
[TearDown]
public void Cleanup()
{
    if (!IgnoreAddResults)
    {
        _title = TestContext.CurrentContext.Test.Properties.Get("title")?.ToString();
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
```

## Test Report

```C#
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
            Title = _title,
            Cases = cases
        }).Result.Result.Id;

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
```

## Example Test

```C#
[Property("code", "your project code here")]
  public class QaseTest1 : Hooks
  {
    [Test, Property("caseid", "your case id here")]
    [Property("title", "your title test")]
    public void GoogleTest()
    {
      Driver.Navigate().GoToUrl("http://www.google.com");
      Driver.FindElement(By.Name("q")).SendKeys("Selenium");
      System.Threading.Thread.Sleep(5000);
      Driver.FindElement(By.Name("btnK")).Click();
      Assert.AreEqual(true, Driver.FindElement(By.Id("btnN")).Displayed);
      Assert.That(Driver.PageSource.Contains("Selenium"), Is.EqualTo(true), "The text selenium doest not exist");
    }
  }
}
```

## Running tests with test explorer

https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019

