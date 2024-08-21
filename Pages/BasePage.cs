using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationInTesting.Pages
{
    public class BasePage
    {
        protected IWebDriver driver;

        private const int timeOutSec = 10;

        private IWebDriver Driver => driver;

        private WebDriverWait wait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;

            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOutSec));
        }

        public void RefreshPage()
        {
            driver.Navigate().Refresh();
        }
    }
}
