using AutomationInTesting.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace AutomationInTesting.Steps
{
    [Binding]
    public class FrontPageSteps : BaseSteps
    {
        private static FrontPage frontPage = new FrontPage(Driver.WebDriver);

        public FrontPageSteps(ScenarioContext scenarioContext, FeatureContext featureContext)
            : base(scenarioContext, featureContext)
        {
        }

        [Given("I make a reservation for the next month, for '(.*)' days, starting on the first available '(.*)' of the month")]
        public void MakeReservationForNextMonthStartingonFirstDay(int duration, string weekDay)
        {
            scenarioContext.Add("isCreated", false);

            frontPage.BookARoom();

            // in case the reservation can not be created, the same step can be repeted until the reservation is made
            var reservationDates = frontPage.CreateReservationForNextMonth(weekDay, duration);
            scenarioContext.Add("weekDay", weekDay);
            scenarioContext.Add("reservationDuration", duration);
            scenarioContext.Add("reservationDates", reservationDates);
        }

        [Given("I fill the booking form with '(.*)', '(.*)', '(.*)', '(.*)'")]
        public void FilWithContactInformation(string firstName, string lastName, string email, string phone)
        {
            var clientData = new ClientInformationModel() { FirstName = firstName, LastName = lastName, Email = email, Phone = phone };
            frontPage.FillClientContactInformation(clientData);
            bool isCreated = frontPage.BookReservation();

            if (isCreated)
            {
                scenarioContext["isCreated"] = true;
                return;
            }
            else
            {
                scenarioContext.Add("datesAreUnavailableError", frontPage.BookingDatesAreUnabailable());

                // add in the scenarioContext what kind of error poped up
            }
        }

        [Given("if the reservation is unsuccessful, I postpone the reservation one more month")]
        public void PostponeReservationIfNotCreated()
        {
            scenarioContext.TryGetValue("isCreated", out bool isCreated);
            var notCreatedBecauseUnavailableDates = !isCreated && scenarioContext.TryGetValue("datesAreUnavailableError", out bool value) && value;
            if (notCreatedBecauseUnavailableDates)
            {
                //The room dates are either invalid or are already booked for one or more of the dates that have been selected.
                scenarioContext.TryGetValue("weekDay", out string weekDay);
                scenarioContext.TryGetValue("reservationDuration", out int duration);

                var newReservationDates = frontPage.CreateReservationForNextMonth(weekDay, duration);

                scenarioContext["reservationDates"] = newReservationDates;
                scenarioContext["isCreated"] = frontPage.BookReservation();
                Methods.ExplicitWait(500);
            }
        }

        [Then("the reservation is created")]
        public void ReservationIsCreated()
        {
            scenarioContext.TryGetValue("isCreated", out bool isCreated);

            if (isCreated)
            {
                scenarioContext.TryGetValue("reservationDates", out (string, string) reservationDates);
                bool succesfullyCreated = frontPage.ReservationSuccesfullyCreatedforDates(reservationDates.Item1, reservationDates.Item2);
                if (succesfullyCreated)
               frontPage.CloseSuccesWindow();

                Assert.IsTrue(succesfullyCreated);
            }
            else
            {
                Assert.IsTrue(false, "The reservation could not be created");
            }
        }

        [AfterTestRun]
        public static void CloseBrows()
        {
            Driver.WebDriver.Close();
        }
    }
}