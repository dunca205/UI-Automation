using AutomationInTesting.Pages;
using TechTalk.SpecFlow;

namespace AutomationInTesting.Steps
{
    public class BaseSteps
    {
        public ScenarioContext scenarioContext;
        public FeatureContext featureContext;
        private AdminPage adminPage = new AdminPage(Driver.WebDriver);
        private FrontPage frontPage = new FrontPage(Driver.WebDriver);
        private BasePage basePage = new BasePage(Driver.WebDriver);

        public BaseSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            this.scenarioContext = scenarioContext;
            this.featureContext = featureContext;
        }

        //  My approach was to add this step to this class because is not page specific and I`m not duplicating the steps for each page
        [Given("I navigate to the Shady Meadows '(.*)' page")]
        public void NagivateToPage(string pageType)
        {
            switch (pageType)
            {
                case "Front":
                    {
                        frontPage.NavigateToFrontPage();
                        break;
                    }

                case "Admin":
                    {
                        adminPage.NavigateToAdminPage();
                        break;
                    }
            }
        }

        /// <summary>
        /// Required when running multiple scenarios in RoomReservationFearure
        /// </summary>
        [AfterScenario]
        [Scope(Tag = "RefreshAfterSceario")]
        public void RefreshPage()
        {
            basePage.RefreshPage();
        }
    }
}