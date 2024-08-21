Feature: RoomManagement

As an Admin, I would like to be able to add new rooms to the website and be able to edit the rooms once added
@LogoutAdminUser
Scenario: Admin can add a new room to the system
  As an Admin, I want to be able to add new rooms to the system with different options
	Given I navigate to the Shady Meadows '<Page>' page
	When I login with the following credentials '<Username>' and '<Password>'
	And I create a '<Room Type>' room that includes amenities such as '<Amenities>', the room is accesible '<Accessible>' and the price is '<Price>'
	Then the room '<Creation Status>' succesfully added to the system

Examples:
	| Page  | Username | Password | Room Type | Amenities            | Accessible | Price | Creation Status |
	| Admin | admin    | password | Double    | Refreshments,TV,Safe |            |       | is              |
	| Admin | admin    | password | Single    | Radio,Views,WiFi     | true       | 150   | is              |
	| Admin | admin    | password | Family    | TV,Views             | false      | 999   | is              |
	| Admin | admin    | password | Family    | TV,Views             | false      | 1000  | is not          |
	| Admin | admin    | password | Twin      |                      |            |       | is              |
	# the decimal price value is not supported redirects the user to a black page and throws an error on the console
	#(TypeError: Cannot read properties of undefined (reading 'length'))

@LogoutAdminUser
Scenario: Admin user can edit existing room
 As an Admin, I want to be able to update the description and the image of a room I just created
	Given I navigate to the Shady Meadows '<Page>' page
	When I login with the following credentials '<Username>' and '<Password>'
	And I create a '<Room Type>' room that includes amenities such as '<Amenities>', the room is accesible '<Accessible>' and the price is '<Price>'
	And I select the created room
	And I update the room description with '<Room Description Update>' and I update the Image with '<Image Update>'
	Then the room has succesuflly been updated

Examples:
	| Page  | Username | Password | Room Type | Amenities            | Accessible | Price | Room Description Update  | Image Update                                                                          |
	| Admin | admin    | password | Double    | Refreshments,TV,Safe | true       | 200   | Updated room description | https://codeskulptor-demos.commondatastorage.googleapis.com/GalaxyInvaders/back02.jpg |