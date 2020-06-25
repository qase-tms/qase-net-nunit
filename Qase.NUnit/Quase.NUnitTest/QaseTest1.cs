using NUnit.Framework;
using OpenQA.Selenium;
using Quase.NUnitTest.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quase.NUnitTest
{

  [Property("code", "TEST")]//your project code here
  public class QaseTest1 : Hooks
  {
    [Test, Property("caseid", "1")] //your case id here
    [Property("title", "Google")] //your title test
    public void GoogleTest()
    {
      Driver.Navigate().GoToUrl("http://www.google.com");
      Driver.FindElement(By.Name("q")).SendKeys("Selenium");
      System.Threading.Thread.Sleep(5000);
      Driver.FindElement(By.Name("btnK")).Click();
      Assert.AreEqual(true, Driver.FindElement(By.Id("btnN")).Displayed);
      Assert.That(Driver.PageSource.Contains("Selenium"), Is.EqualTo(true), "The text selenium doest not exist");
    }

    [Test, Property("caseid", "1")] //your case id here
    [Property("title", "Executeautomation")] //your title test
    public void ExecuteAutomationTest()
    {
      Driver.Navigate().GoToUrl("http://executeautomation.com/demosite/Login.html");
      Driver.FindElement(By.Name("UserName")).SendKeys("admin");
      Driver.FindElement(By.Name("Password")).SendKeys("admin");
      Driver.FindElement(By.Name("Login")).Submit();
      System.Threading.Thread.Sleep(2000);
      Assert.That(Driver.PageSource.Contains("Selenium"), Is.EqualTo(true), "The text selenium doest not exist");
    }
  }
}
