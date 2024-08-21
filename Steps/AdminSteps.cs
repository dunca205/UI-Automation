using AutomationInTesting.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace AutomationInTesting.Steps
{
    [Binding]
    public class AdminSteps
    {
        private static AdminPage adminPage = new AdminPage(Driver.WebDriver);
        private const string defaultPrice = "100";
        private ScenarioContext scenarioContext;
        private FeatureContext featureContext;

        public AdminSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            this.scenarioContext = scenarioContext;
            this.featureContext = featureContext;
        }

        [When("I login with the following credentials '(.*)' and '(.*)'")]
        public void LoginWithGivenCredentials(string username, string password)
        {
            Methods.ExplicitWait(1000);
            adminPage.LoginWithUsernameAndPassword(username, password);
        }

        [When("I create a '(.*)' room that includes amenities such as '(.*)', the room is accesible '(.*)' and the price is '(.*)'")]
        public void CreateRoomBasedOnGivenData(string roomType, string amenities, string isAccessible, string price)
        {
            var listOfamenities = !String.IsNullOrEmpty(amenities) ? amenities.Split(',').ToList() : new List<string>();
            var roomDetails = new RoomModel()
            {
                RoomName = Methods.GenerateRandomNumber(3),
                Price = !string.IsNullOrEmpty(price) ? price : defaultPrice,
            };
            roomDetails.RoomType = roomType;
            roomDetails.RoomDetails = listOfamenities;
            roomDetails.IsAccessible = !String.IsNullOrEmpty(isAccessible) ? isAccessible : "false";
            adminPage.CreateNewRoom(roomDetails);
            scenarioContext.Add("roomName", roomDetails.RoomName);
            scenarioContext.Add("room", roomDetails);
        }

        [When("I select the created room")]
        public void SelectCreatedRoom()
        {
            scenarioContext.TryGetValue("roomName", out string roomName);
            adminPage.SelectRoom(roomName);
        }

        [When("I update the room description with '(.*)' and I update the Image with '(.*)'")]
        public void UpdateRoom(string roomDescription, string imageUrl)
        {
            //other than this room picture https://www.mwtestconsultancy.co.uk/img/testim/room1.jpg
            // I found the following jpg pictures
            //https://codeskulptor-demos.commondatastorage.googleapis.com/GalaxyInvaders/back04.jpg
            //https://codeskulptor-demos.commondatastorage.googleapis.com/GalaxyInvaders/back02.jpg
            adminPage.UpdateRoom(roomDescription, imageUrl);
            scenarioContext.Add("descriptionUpdated", roomDescription);
            scenarioContext.Add("imageUpdated", imageUrl);
        }

        [Then("the room '(.*)' succesfully added to the system")]
        public void RoomIsOrNotSuccesfullySaved(string creationStatus)
        {
            scenarioContext.TryGetValue("room", out RoomModel createdRoom);
            Methods.ExplicitWait(1000);
            if (creationStatus == "is")
            {
                Assert.IsTrue(adminPage.RoomIsCreated(createdRoom));
            }
            else
            {
                Assert.IsFalse(adminPage.RoomIsCreated(createdRoom), "The room could not be added to the System");
            }
        }

        [Then("the room has succesuflly been updated")]
        public void RoomIsUpdatedSaved()
        {
            scenarioContext.TryGetValue("descriptionUpdated", out string roomDescription);
            scenarioContext.TryGetValue("imageUpdated", out string imageUrl);
            Assert.IsTrue(adminPage.ValidateRoomImageAndDescription("Description: " + roomDescription, imageUrl));
        }

        [BeforeScenario]
        [Scope(Tag = "LogoutAdminUser")]
        public void LogoutAdminUser()
        {
            Methods.ExplicitWait(1000);
            adminPage.LogOutIfUserIsLoggedIn();
        }
    }
}