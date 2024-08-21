using AutomationInTesting.Pages;
using OpenQA.Selenium;

namespace AutomationInTesting
{
    public class BookingManagementTableView : BasePage
    {
        private string TABLEROWSXPATH = "div[data-type='room']";

        public BookingManagementTableView(IWebDriver driver)
            : base(driver)
        {
        }

        public List<RoomModel> GetRoomsDetailsFromTableRows()
        {
            List<RoomModel> existingRooms = new List<RoomModel>();

            var tableRows = driver.FindElements(By.CssSelector(TABLEROWSXPATH));

            foreach (var tableRow in tableRows) // iteratre through all tableRows and get the details of each tableRow 
            {
                var existingRoom = new BookingManagementTableRow { TableRow = tableRow };
                existingRooms.Add(existingRoom.GetRoomDetails());
            }

            return existingRooms;
        }
    }

    public class BookingManagementTableRow
    {
        private string ROOMNAMELOCATOR = "p[id^='roomName']";
        private string ROOMTYPELOCATOR = "p[id^='type']";
        private string ACCESIBLELOCATOR = "p[id^='accessible']";
        private string ROOMPRICELOCATOR = "p[id^='roomPrice']";
        private string ROOMDETAILS = "p[id^='details']";

        public BookingManagementTableRow()
        {
        }

        public required IWebElement TableRow { get; set; }

        private string RowRoomName => TableRow.FindElement(By.CssSelector(ROOMNAMELOCATOR)).Text;

        private string RowType => TableRow.FindElement(By.CssSelector(ROOMTYPELOCATOR)).Text;

        private string RowAccesible => TableRow.FindElement(By.CssSelector(ACCESIBLELOCATOR)).Text;

        private string RowPrice => TableRow.FindElement(By.CssSelector(ROOMPRICELOCATOR)).Text;

        private List<string> RowRoomDetails => TableRow.FindElement(By.CssSelector(ROOMDETAILS)).Text.Split(',').ToList();

        public RoomModel GetRoomDetails()
        {
            return new RoomModel
            {
                RoomName = RowRoomName,
                RoomType = RowType,
                IsAccessible = RowAccesible,
                Price = RowPrice,
                RoomDetails = RowRoomDetails,
            };
        }
    }
}
