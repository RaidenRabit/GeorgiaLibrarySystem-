using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using NUnit.Framework;

namespace Tests.AcceptanceTest
{
    
    [Category("Acceptance")]
    [Property("Priority", 1)]
    public class LendingTest : AcceptanceTestBase
    {
        [Test]
        public void As_a_Member_With_too_many_books_I_Cant_Borrow_Books()
        {
            //Arrange
            string ssn = "123456789", password = "test";
            IWebElement ssnBox = _chromeDriver.FindElement(By.Id("SSN"));
            IWebElement passwordBox = _chromeDriver.FindElement(By.Id("Password"));
            IWebElement originalhWebsiteText = _chromeDriver.FindElements(By.ClassName("nav-link"))[0]; //get initial 'About' element
            ssnBox.SendKeys(ssn);
            passwordBox.SendKeys(password);
            _chromeDriver.FindElement(By.Id("Login")).Click();
            _chromeDriver.FindElements(By.ClassName("nav-link"))[3].Click();
            WaitForStaleness(originalhWebsiteText);

            IWebElement baseTable = _chromeDriver.FindElement(By.Id("materialTable"));
            IList<IWebElement> rows = baseTable.FindElements(By.TagName("td"));

            //Act
            rows[7].Click();
            WaitForStaleness(rows[7]);
          
            //Assert
            try
            {
                _chromeDriver.FindElement(By.Id("failedBorrow"));
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        public void As_a_Member_Without_too_many_books_I_Can_Borrow_Books()
        {
            //Arrange
            string ssn = "123456788", password = "test";
            IWebElement ssnBox = _chromeDriver.FindElement(By.Id("SSN"));
            IWebElement passwordBox = _chromeDriver.FindElement(By.Id("Password"));
            IWebElement originalhWebsiteText = _chromeDriver.FindElements(By.ClassName("nav-link"))[0]; //get initial 'About' element
            ssnBox.SendKeys(ssn);
            passwordBox.SendKeys(password);
            _chromeDriver.FindElement(By.Id("Login")).Click();
            _chromeDriver.FindElements(By.ClassName("nav-link"))[3].Click();
            WaitForStaleness(originalhWebsiteText);

            IWebElement baseTable = _chromeDriver.FindElement(By.Id("materialTable"));
            IList<IWebElement> rows = baseTable.FindElements(By.TagName("td"));

            //Act
            rows[7].Click();
            WaitForStaleness(rows[7]);

            //Assert
            try
            {
                _chromeDriver.FindElement(By.Id("succesfullBorrow"));
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
    }
}
