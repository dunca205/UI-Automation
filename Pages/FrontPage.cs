using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace AutomationInTesting.Pages
{
    public class FrontPage : BasePage
    {
        private const string FrontPageURL = "https://automationintesting.online/#";
        private const string BOOKTHISROOMBUTTON = "//button[@type='button' and text()='Book this room']";
        private const string FIRSTNAME = "firstname";
        private const string LASTNAME = "lastname";
        private const string EMAIL = "email";
        private const string PHONENUMBER = "phone";
        private const string BOOKBUTTONACTION = "//button[@type='button' and text()='Book']";
        private const string NEXTMONTHBUTTON = "//button[@type='button' and text()='Next']";
        private const string CURRENTMONTHONCALENDAR = "//span[@class='rbc-toolbar-label']";
        private const string CALENDARHEADER = "//div[@class='rbc-row rbc-month-header']//div//span";
        private const string CALENDARROWS = "//div[@class='rbc-month-row']";
        private const string ROWSVAL = "//div[@class='rbc-row-content']//div//div";
        private const string CALENDARCELL = "//div[contains(@class, 'rbc-date-cell')]//button[text()='NUMBER']";
        private const string ERRORMESSAGE = "//div[@class='alert alert-danger']/p";
        private const string CONTACTNAME = "//input[@data-testid='ContactName']";
        private const string SUCCESFULLYBOOKEDLOCATORS = "//div[@class='col-sm-6 text-center']//p";
        private const string CLOSESUCCESMODAL = "//button[text()='Close']";
        private const string NUMBERPLACEHOLDER = "NUMBER";
        private const string RoomDatesUnavailableMessage = "The room dates are either invalid or are already booked for one or more of the dates that you have selected.";
        private const string BookingSuccessfulMessage = "Booking Successful!";
        private const string CongratulationsMessage = "Congratulations! Your booking has been confirmed for:";

        public FrontPage(IWebDriver driver)
            : base(driver)
        {
        }

        protected IWebElement BookThisRoomButton => driver.FindElementOnPage(By.XPath(BOOKTHISROOMBUTTON));

        protected IWebElement FirstNameField => driver.FindElementOnPage(By.Name(FIRSTNAME));

        protected IWebElement LastNameField => driver.FindElementOnPage(By.Name(LASTNAME));

        protected IWebElement EmailField => driver.FindElementOnPage(By.Name(EMAIL));

        protected IWebElement PhoneNumberField => driver.FindElement(By.Name(PHONENUMBER));

        protected IWebElement BookButtonAction => driver.FindElementOnPage(By.XPath(BOOKBUTTONACTION));

        protected IWebElement NextMonthButton => driver.FindElementOnPage(By.XPath(NEXTMONTHBUTTON));

        protected IWebElement ActualMonthOnCalendar => driver.FindElement(By.XPath(CURRENTMONTHONCALENDAR));

        protected IWebElement ContactName => driver.FindElement(By.XPath(CONTACTNAME));

        public void BookARoom()
        {
            BookThisRoomButton.ClickOnElement();
        }

        public string MoveToNextMonth()
        {
            NextMonthButton.ClickOnElement();
            return ActualMonthOnCalendar.Text;
        }

        public (string, string) CreateReservationForNextMonth(string weekDay, int duration)
        {
            Methods.ScrollToElememnt(driver, BookButtonAction);
            string currentMonth = MoveToNextMonth();

            string possibleStartDate = FindStartDateOfReservationOnCurrentMonth(weekDay[..3]);
            string possibleEndDate = "";

            if (!string.IsNullOrEmpty(possibleStartDate))
            {
                possibleEndDate = (int.Parse(possibleStartDate) + duration).ToString(); // calculates the possible endDate of the reservation
                if (possibleEndDate.Length == 1)
                    possibleEndDate = '0' + possibleEndDate;

                SelectIntervalResrvation(possibleStartDate, possibleEndDate);
            }

            return (possibleStartDate + ' ' + currentMonth, possibleEndDate + ' ' + currentMonth);
        }

        public void FillClientContactInformation(ClientInformationModel clientInformation)
        {
            FirstNameField.SendKeys(clientInformation.FirstName);
            LastNameField.SendKeys(clientInformation.LastName);
            EmailField.SendKeys(clientInformation.Email);
            PhoneNumberField.SendKeys(clientInformation.Phone);
        }

        public string FindStartDateOfReservationOnCurrentMonth(string weekday)
        {
            Methods.ScrollToElememnt(driver,BookButtonAction);
            int rowsToCheck = 2;
            int indexForWeekDayColumn = GetHeaderIndexForDayWeek(weekday); // returns the index associated to each weekday

            // if the startDay is not on first row, must be on the second row
            for (int rowNr = 1; rowNr <= rowsToCheck; rowNr++)
            {
                var currentRowXpath = CALENDARROWS + $"[{rowNr}]" + ROWSVAL; // this xpath contains 7 child elements representing each weekday for week i
                var xpathForCurrentRowAndWeekDay = currentRowXpath + $"[{indexForWeekDayColumn}]";

                var element = driver.FindElementOnPage(By.XPath(xpathForCurrentRowAndWeekDay));

                if (!element.GetAttribute("class").Contains("off-range")) // validation that the day from the current week is from the current month
                {
                    var startDay = driver.FindElementOnPage(By.XPath(xpathForCurrentRowAndWeekDay + "//button")).Text; // get the number from the calendar cell
                    return startDay; // return the startDay of the reservation
                }
            }

            return string.Empty;
        }

        // this step was very difficut from my point of view because none of the action methods worked as I expected,
        public bool SelectIntervalResrvation(string startDay, string endDay)
        {
            var startDate = driver.FindElementOnPage(By.XPath(CALENDARCELL.Replace(NUMBERPLACEHOLDER, startDay)));
            var endDate = driver.FindElementOnPage(By.XPath(CALENDARCELL.Replace(NUMBERPLACEHOLDER, endDay)));

            Actions actions = new Actions(driver);
            actions.ClickAndHold(startDate)
                    .Pause(TimeSpan.FromSeconds(2))
                    .MoveToElement(endDate)
                    .Pause(TimeSpan.FromSeconds(2))
                    .Build().Perform();
            Methods.ExplicitWait(500);

            actions.ClickAndHold(endDate).Pause(TimeSpan.FromSeconds(2))
                    .MoveToElement(startDate).Pause(TimeSpan.FromSeconds(2))
                    .Release().Build().Perform();

            Methods.ExplicitWait(2000);

            return true;
        }

        public bool BookingDatesAreUnabailable()
        {
            var reservationNotSaved = driver.FindElements(By.XPath(ERRORMESSAGE));
            if (reservationNotSaved.Count() == 0)
            {
                return false; // no errors were thrown
            }

            return reservationNotSaved.First().Text == RoomDatesUnavailableMessage;
        }

        public bool BookReservation()
        {
            BookButtonAction.ClickOnElement();
            Methods.ExplicitWait(500);
            var errorsDisplayed = driver.FindElements(By.XPath(ERRORMESSAGE)); // verifies if any error popped up
            if (errorsDisplayed.Any())
            Methods.ScrollToElememnt(driver, errorsDisplayed.Last()); 

            return !errorsDisplayed.Any();
        }

        public bool ReservationSuccesfullyCreatedforDates(string startDay, string endDay)
        {
            // on the Succes message, are displayed the CheckIn and CheckOut dates => CheckOut date is endDay + 1
            endDay = CalculateCheckoutDate(endDay);
            var formatedDate = Methods.FormateDate(startDay, endDay);

            var successMessagePopup = driver.FindElements(By.XPath(SUCCESFULLYBOOKEDLOCATORS));
            if (successMessagePopup.Count() == 0)
            {
                return false;
            }

            var congratulationMessage = successMessagePopup.First().Text == CongratulationsMessage;
            var dateConfirmation = successMessagePopup.Last().Text == formatedDate;

            Methods.ExplicitWait(500);

            return congratulationMessage && dateConfirmation;
        }

        public void CloseSuccesWindow()
        {
            var closeSuccesWindow = driver.FindElementOnPage(By.XPath(CLOSESUCCESMODAL));
            closeSuccesWindow.Click();
        }

        private string CalculateCheckoutDate(string endDay)
        {
            // 06 June 2026
            var dateStrings = endDay.Split(' ');

            int checkOutIntDay = Convert.ToInt16(dateStrings[0]) + 1;

            endDay = checkOutIntDay < 10 ? $"0{checkOutIntDay}" : checkOutIntDay.ToString();

            return endDay + ' ' + dateStrings[1] + ' ' + dateStrings[2];
        }

        // this method iterates through the coulmn header and returns the index of the given weekday once is found
        private int GetHeaderIndexForDayWeek(string day)
        {
            var weekDays = driver.FindElements(By.XPath(CALENDARHEADER));
            foreach (var weekDay in weekDays)
            {
                if (weekDay.Text == day)
                {
                    return weekDays.IndexOf(weekDay) + 1;
                }
            }

            return -1;
        }

        public void NavigateToFrontPage()
        {
            driver.Navigate().GoToUrl(FrontPageURL);
        }
    }
}
