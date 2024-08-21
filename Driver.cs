using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationInTesting
{
    public class Driver
    {
        private static IWebDriver driver;
        private static int timeoutSeconds = 15;

        public static IWebDriver WebDriver
        {
            get
            {
                if (driver == null)
                {
                    // the DragAndDrop action was not working and I verified if it behaves the same when using other browserwhich was not
                    // driver = new EdgeDriver();
                    driver = new ChromeDriver();
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeoutSeconds);
                    driver.Manage().Window.Maximize();
                }

                return driver;
            }

            set
            {
                driver = value;
            }
        }
    }
}
