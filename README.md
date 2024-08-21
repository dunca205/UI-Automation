This is a small personal project I worked on to practice and improve my automation testing skills.

Requirement 1: Visit the website https://automationintesting.online/ and reserve a room for the upcoming month. To do this, you need to select the first available booking date, which begins on the first Monday of the month and lasts for four consecutive days. If any of those days are unavailable, the booking period will be postponed to the following month. Once the reservation is made, you need to verify that it has been successfully created. 

Scenario 1: Room Reservation
Objective: This scenario automates the process of booking a room on the website.
 The automation script visits the website and navigates to the booking section.
 It identifies the first available booking date starting on the first Monday of the upcoming month.
 The script attempts to reserve the room for four consecutive days.
 If the selected dates are unavailable, it postpones the booking to the next month and retries.
 Once a booking is successful, the script verifies the reservation by checking for a confirmation message.
Outcome: This ensures that a room is successfully booked starting from the first Monday of an available month, with the booking process being verified for accuracy.

Requirement 2: Next, log in to the admin portal at https://automationintesting.online/#/admin/ using the following credentials: admin / password. Once you're logged in, add a double room to the system that includes amenities such as refreshments, a TV, and a safe. After the room has been added, you need to update both its description and image. 

Scenario 2: Admin Portal Room Management
Objective: This scenario automates the process of adding and updating a room in the admin portal.
  The script logs into the admin portal using the provided credentials.
  It navigates to the room management section and adds a new double room, including amenities like refreshments, a TV, and a safe.
  After the room is added, the script updates the room’s description and image to provide a more detailed and accurate representation.
  The script then verifies that the new room is properly added and updated in the system.
Outcome: This scenario ensures that a room is correctly added and its details are up-to-date, reflecting any changes in the description or imagery.
