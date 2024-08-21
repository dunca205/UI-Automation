using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationInTesting.Pages
{
    public class AdminPage : BasePage
    {
        private const string AdminPageURL = "https://automationintesting.online/#/admin/";
        private const string ADMINUSERNAME = "username";
        private const string ADMINPASSWORD = "password";
        private const string LOGINBUTTON = "doLogin";
        private const string ROOMNAME = "roomName";
        private const string ROOMTYPE = "select[id='type']";
        private const string ACCSSESIBLEVALUE = "select[id='accessible']";
        private const string ROOMPRICE = "roomPrice";
        private const string ROOMAMENITIES = "input[value='placeholder']";
        private const string CREATEBUTTON = "createRoom";
        private const string SELECTROOM = "//div[@data-testid='roomlisting']//p[text()='placeholder']";
        private const string EDITROOMBUTTON = "//button[text()='Edit']";
        private const string DESCRIPTIONFIELD = "textarea[id='description']";
        private const string IMAGEURLFIELD = "image";
        private const string UPDATEBUTTON = "update";
        private const string LOGOUT = "//a[text()='Logout']";
        private const string ROOMDESCRIPTION = "//div[contains(p/text(), 'Description:')]";
        private const string IMAGEURL = "//div[contains(p/text(), 'Image:')]//img";
        private const string NoFeaturesAdded = "No features added to the room";
        private BookingManagementTableView RoomManagementTable;

        private const string placeholder = "placeholder";

        public AdminPage(IWebDriver driver)
            : base(driver)
        {
            RoomManagementTable = new BookingManagementTableView(driver);
        }

        private IWebElement AdminUsernameField => driver.FindElementOnPage(By.Id(ADMINUSERNAME));

        private IWebElement AdminPasswordField => driver.FindElementOnPage(By.Id(ADMINPASSWORD));

        private IWebElement LoginButton => driver.FindElementOnPage(By.Id(LOGINBUTTON));

        private IWebElement RoomName => driver.FindElementOnPage(By.Id(ROOMNAME));

        private IWebElement RoomType => driver.FindElementOnPage(By.CssSelector(ROOMTYPE));

        private IWebElement IsAccesible => driver.FindElementOnPage(By.CssSelector(ACCSSESIBLEVALUE));

        private IWebElement RoomPrince => driver.FindElementOnPage(By.Id(ROOMPRICE));

        private IWebElement SubmitRoomRegistration => driver.FindElementOnPage(By.Id(CREATEBUTTON));

        private IWebElement EditRoom => driver.FindElementOnPage(By.XPath(EDITROOMBUTTON));

        private IWebElement RoomDescription => driver.FindElementOnPage(By.CssSelector(DESCRIPTIONFIELD));

        private IWebElement ImagieURL => driver.FindElementOnPage(By.Id(IMAGEURLFIELD));

        private IWebElement UpdateButton => driver.FindElementOnPage(By.Id(UPDATEBUTTON));

        public void LoginWithUsernameAndPassword(string username, string password)
        {
            AdminUsernameField.SendKeys(username);

            AdminPasswordField.SendKeys(password);

            LoginButton.ClickOnElement();
        }

        public void NavigateToAdminPage()
        {
            driver.Navigate().GoToUrl(AdminPageURL);
        }

        public void CreateNewRoom(RoomModel newRoom)
        {
            RoomName.SendKeys(newRoom.RoomName);

            if (!string.IsNullOrEmpty(newRoom.RoomType))
            {
                var selectRoomType = new SelectElement(RoomType);
                selectRoomType.SelectByValue(newRoom.RoomType);
            }

            var isRoomAccesible = new SelectElement(IsAccesible);
            isRoomAccesible.SelectByValue(newRoom.IsAccessible);

            RoomPrince.SendKeys(newRoom.Price);

            if (newRoom.RoomDetails.Any())
            {
                foreach (var amenity in newRoom.RoomDetails)
                {
                    var roomAmenity = driver.FindElementOnPage(By.CssSelector(ROOMAMENITIES.Replace(placeholder, amenity)));
                    roomAmenity.ClickOnElement();
                }
            }

            SubmitRoomRegistration.ClickOnElement();
            Methods.ScrollToElememnt(driver, SubmitRoomRegistration);
        }

        public void SelectRoom(string roomName)
        {
            var selectTargetRoom = driver.FindElementOnPage(By.XPath(SELECTROOM.Replace(placeholder, roomName)));

            selectTargetRoom.ClickOnElement();
        }

        public void UpdateRoom(string roomDescription, string roomImageUrl)
        {
            EditRoom.ClickOnElement();

            RoomDescription.Clear();
            RoomDescription.SendKeys(roomDescription);

            ImagieURL.Clear();
            ImagieURL.SendKeys(roomImageUrl);

            UpdateButton.ClickOnElement();
        }

        public bool RoomIsCreated(RoomModel roomToCheck)
        {
            bool isMatching = false;

            var existingRooms = RoomManagementTable.GetRoomsDetailsFromTableRows(); // the list is used so that each RoomModel to be compared with the RoomModel roomToCheck
            foreach (var room in existingRooms)
            {
                isMatching = IsMatchWithExistingRoom(room, roomToCheck);
                if (isMatching)
                {
                    return true; // once a match is found return true, and stop iterating
                }
            }

            return isMatching; // if no match is found, the variable isMatching will not have its value changed => false
        }

        /// <summary>
        /// Method is required when running the same scenario multiple times with different data
        /// When I ran the tests for the first time, I observed that tests are failing when they`re ran toghether
        /// but passing when ran individually, after I created + used the method LogOutIfUserIsLoggedIn
        /// </summary>
        public void LogOutIfUserIsLoggedIn()
        {
            // I used the method FindElements in case the user is not logged to not throw an exception 
            var elements = driver.FindElements(By.XPath(LOGOUT));
            if (elements.Any())
            {
                elements[0].Click();
            }
        }

        public bool ValidateRoomImageAndDescription(string expectedDescription, string expectedImageUrl)
        {
            var actualDescriptionText = driver.FindElementOnPage(By.XPath(ROOMDESCRIPTION)).Text;
            var actualImageUrlText = driver.FindElementOnPage(By.XPath(IMAGEURL)).GetAttribute("src");

            return actualDescriptionText == expectedDescription && actualImageUrlText == expectedImageUrl;
        }

        private bool IsMatchWithExistingRoom(RoomModel existingRoom, RoomModel createdRoom)
        {
            bool roomDetailsMatch = false;

            if (createdRoom.RoomDetails.Count == 0) // if room does not contain any features then the existingRoom must have the NoFeaturesAdded string as Room details
            {
                roomDetailsMatch = existingRoom.RoomDetails.First() == NoFeaturesAdded;
            }
            else
            {
                foreach (var detail in createdRoom.RoomDetails) // checks if all details of the createdRoom are present in the list of details associated with the existingRoom
                {
                    roomDetailsMatch = existingRoom.RoomDetails.Select(detail => detail.TrimStart(' ')).Contains(detail);

                    if (!roomDetailsMatch)
                    {
                        return false;
                    }
                }
            }

            return
                existingRoom.RoomName == createdRoom.RoomName &&
                existingRoom.RoomType == createdRoom.RoomType &&
                existingRoom.IsAccessible == createdRoom.IsAccessible &&
                existingRoom.Price == createdRoom.Price &&
                roomDetailsMatch;
        }
    }
}
