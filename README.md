# [Qase TMS](https://qase.io) NUnit Test

### Configuration

appsettings.json configuration:

```json
{
  "UrlAPI": "https://api.qase.io/v1",
  "Api_Token": "your api token"
}
```

### Usage

Inherit from the TestBase class in each of your test classes.
 
```
public class MyTests : TestBase
{
  ...
}
```

In your test class include the Qase project code [coded].

```
[Property("code", "your project code here")]
public class MyTests : TestBase
{
  ...
}
```

Include the Qase case id [caseid] and titel test [title] as a property to the test method. The steps contained in the test method should map to the test steps in the Qase test case.

```
[Test, Property("caseid", "your case id here")]
[Property("title", "your title test")]
public void GoogleTest()
{
  ..
}
```

Execute the tests in your test class. 
Open the project in Qase and view the test runs. 
The results were updated by executing the NUnit tests.

#### Properties empty when using parameterized test in NUnit
- Please see https://stackoverflow.com/questions/47434571/nunit-test-properties-not-accessible-in-parametrized-tests

### For a detailed description of the QaseAPI library methods
- Please see https://github.com/qase-tms/qase-net-api