
Feature: RoomReservation

As a user I want to create a new room reservation
So that I can book a room starting on the first available specific weekday of next month and last for x days 
And if there are no available dates for the specified criteria
I should be able to postpone the reservation to next month with the same reservation criteria	
@RefreshAfterSceario
Scenario: Visit the webpage and verify available dates
	Given I navigate to the Shady Meadows '<Page>' page
	And I make a reservation for the next month, for '<Reservation Duration>' days, starting on the first available '<Day>' of the month
	And I fill the booking form with '<Firstname>', '<Lastname>', '<Email>', '<Phone>'
	But if the reservation is unsuccessful, I postpone the reservation one more month
	Then the reservation is created


Examples:
	| Page  | Reservation Duration | Day    | Firstname          | Lastname           | Email                       | Phone         |
	| Front | 2                    | Monday | ClientNR1FirstName | ClientNR1LastName  | ClientNR1FirstName@fake.com | +987654321545 |
	| Front | 2                    | Friday | ClientNr2FirstName | ClientNR1 NameForm | ClientNr2FirstName@fake.com | +123456789454 |


