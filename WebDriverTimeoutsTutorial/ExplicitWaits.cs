﻿using System;
using System.Threading;
using AutomationResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WebDriverTimeoutsTutorial;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace WebdriverTimeoutsTutorial
{
    [TestClass]
    [TestCategory("Explicit waits")]
    public class ExplicitWaits
    {
        private IWebDriver _driver;
        By ElementToWaitFor = By.Id("finish");


        [TestInitialize]
        public void Setup()
        {
            _driver = new WebDriverFactory().Create(BrowserType.Chrome);
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Close();
            _driver.Quit();
        }
        [TestMethod]
        public void ExplicitWait1()
        {
            Thread.Sleep(1000);
        }
        [TestMethod]
        public void ExplicitWait2()
        {
            _driver.Navigate().GoToUrl(URL.HiddenElementUrl);
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            IWebElement element = wait.Until((d) =>
            {
                return d.FindElement(By.Id("success"));
            });
        }
        [TestMethod]
        public void Test1_FixedExplicitly()
        {
            _driver.Navigate().GoToUrl(URL.SlowAnimationUrl);
            FillOutCreditCardInfo();
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("go"))).Click();
            Assert.IsTrue(wait.Until(ExpectedConditions.ElementIsVisible(By.Id("success"))).Displayed);
        }

        [TestMethod]
        public void Test3_ExplicitWait_HiddenElement()
        {
            _driver.Navigate().GoToUrl(URL.HiddenElementUrl);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(ElementToWaitFor)).Click();
        }
        
         [TestMethod]
        public void Test4_ExplicitWait_RenderedAfter()
        {
            _driver.Navigate().GoToUrl(URL.ElementRenderedAfterUrl);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            wait.Message = "Tried to find element with ID finish but the element wasn't clickable on the page after 5 seconds";
            try
            {
                var x = wait.Until(ExpectedConditions.ElementToBeClickable(ElementToWaitFor));

            }
            catch (WebDriverTimeoutException)
            {
                _driver.FindElement(By.TagName("button")).Click();
                wait.Timeout = TimeSpan.FromSeconds(10);
                wait.Until(ExpectedConditions.ElementToBeClickable(ElementToWaitFor)).Click();
            }
            
        }
        //Quiz
        //1. open page
        //2. synchronize on slowest loading element
        //3. proceed with actions
         [TestMethod]
        public void HowToCorrectlySynchronize()
        {

            _driver.Navigate().GoToUrl("https://www.ultimateqa.com");
            _driver.Manage().Window.Maximize();
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            var firstSyncElement = By.XPath("//img[@src='https://ultimateqa.com/wp-content/uploads/2018/05/coding-isometric-01.png']");
            wait.Until(ExpectedConditions.ElementIsVisible(firstSyncElement));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Automation Exercises"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='Automation_Practice']")));

             _driver.FindElement(By.LinkText("Big page with many elements")).Click();

            var finalElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class='tve_image wp-image-4039']")));
            Assert.IsTrue(finalElement.Displayed);
        }
        
        private void FillOutCreditCardInfo()
        {
            _driver.FindElement(By.Id("name")).SendKeys("test name");
            _driver.FindElement(By.Id("cc")).SendKeys("1234123412341234");
            _driver.FindElement(By.Id("month")).SendKeys("01");
            _driver.FindElement(By.Id("year")).SendKeys("2020");
        }
    }
}
