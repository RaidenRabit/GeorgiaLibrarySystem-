using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Tests.AcceptanceTest
{
    [Category("Acceptance")]
    public class AcceptanceTestBase
    {
        private HostServices _hostServices;
        protected IWebDriver _chromeDriver;
        private WebDriverWait _wait;

        public AcceptanceTestBase()
        {
            _hostServices = new HostServices();

        }

        protected void WaitForStaleness(IWebElement element)
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(element));
        }

        [SetUp]
        public void TestsSetup()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            _chromeDriver = new ChromeDriver(chromeDriverService, new ChromeOptions());

            _wait = new WebDriverWait(_chromeDriver, TimeSpan.FromSeconds(2));

            _hostServices.StartServer();
            _chromeDriver.Navigate().GoToUrl("http://localhost:55400/");
        }
        
        [TearDown]
        public void TestsTearDown()
        {
            _chromeDriver.Dispose();
            _hostServices.Dispose();
        }
    }
}
