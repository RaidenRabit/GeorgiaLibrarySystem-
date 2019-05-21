using NUnit.Framework;
using OpenQA.Selenium;

namespace Tests.AcceptanceTest
{
    [Category("Acceptance")]
    [Property("Priority", 1)]
    public class LoginTest : AcceptanceTestBase
    {
        [Test]
        public void As_a_User_I_Can_Log_in()
        {
            //Arrange
            string ssn = "123456789", password = "test";
            IWebElement ssnBox = _chromeDriver.FindElement(By.Id("SSN"));
            IWebElement passwordBox = _chromeDriver.FindElement(By.Id("Password"));
            IWebElement navBarElement = _chromeDriver.FindElements(By.ClassName("nav-link"))[0];

            //Act
            ssnBox.SendKeys(ssn);
            passwordBox.SendKeys(password);
            _chromeDriver.FindElement(By.Id("Login")).Click();
            WaitForStaleness(navBarElement);

            //Assert
            string element = _chromeDriver.FindElements(By.ClassName("nav-link"))[0].Text;
            Assert.IsTrue(element.Equals("Home"));
        }
    }
}
