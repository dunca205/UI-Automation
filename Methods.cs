using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace AutomationInTesting
{
    public static class Methods
    {
        private static readonly Random random = new();
        private const int TimeOutForElementSearch = 15;
        private static int retryAttempts = 3;

        public static IWebElement FindElementOnPage(this IWebDriver driver, By element)
        {
            try
            {
                ExplicitWait(500);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TimeOutForElementSearch));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(element));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));

                IWebElement foundWebElement = driver.FindElement(element);

                if (element != null)
                {
                    return foundWebElement;
                }
            }
            catch
            {
                if (retryAttempts >= 0) // in case the element was not visible in the first attempt
                {
                    retryAttempts--;
                    FindElementOnPage(driver, element);
                }
                else
                {
                    throw new Exception("Element could not be found");
                }
            }

            return null;
        }

        public static void ClickOnElement(this IWebElement webElement)
        {
            WebDriverWait wait = new(Driver.WebDriver, TimeSpan.FromSeconds(TimeOutForElementSearch));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(webElement));

            if (webElement.Enabled)
            {
                webElement.Click();
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GenerateRandomNumber(int length)
        {
            string result = string.Empty;
            for (int i = 0; i < length; i++)
            {
                result = string.Concat(result, random.Next(10).ToString());
            }

            return result;
        }

        public static void ExplicitWait(int miliseconds)
        {
            try
            {
                new WebDriverWait(Driver.WebDriver, TimeSpan.FromMilliseconds(miliseconds)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("dummyElement")));
            }
            catch (WebDriverTimeoutException)
            {
            }
        }

        public static void ScrollToElememnt(IWebDriver driver, IWebElement toElement)
        {
            new Actions(driver)
            .ScrollToElement(toElement).Perform();
        }

        public static string FormateDate(string startDate, string endDate)
        {
            string startDateFormated = DateTime.ParseExact(startDate, "dd MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            string endDateFormated = DateTime.ParseExact(endDate, "dd MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            return startDateFormated + " - " + endDateFormated;
        }
    }
}
