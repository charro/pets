Server Engineering Coding Test Release
--------------------------------------


PERSONAL COMMENTS
-----------------
* First of all, I have to say that I really enjoyed the development process with ASP.NET Core. I didn't try it before, and I didn't used ASP.NET since the old IIS Web Server days, and I found it very productive and straightforward, not to say that I was able to do everything from my Linux Mint powered laptop, seamlessly. It felt right and quick from the very beginning. Totally a step in the right direction from Microsoft.

* Only thing I found not so straightforward was creating a set of unit tests for the most basic functionalities. I tried all options (xUnit, NUnit, MsTest...) but it seemed a not so easy to achieve task without creating a Solution (something I didn't want because it would complicate a project that was already nice and simple), so I decided to make a very simple HTML+JQuery frontend instead to show all the functionalities of the API. Maybe I just didn't find the right way to have them working.



INSTRUCTIONS: HOWTO USE IT
--------------------------

1) Easiest way: Get and run the Docker image from my DockerHub account (https://hub.docker.com/r/charro/pets/):


docker pull charro/pets
sudo docker run -d -p 8080:80 --name mypets charro/pets


And then open localhost:8080. You will see the example test web frontend where you can create/delete animals and feed/pet them.

2) Get the code from my github account and build it yourself


git clone https://github.com/charro/pets.git



LITTLE EXPLANATIONS ABOUT DESIGN/TECHNICAL DECISIONS/ASSUMPTIONS
----------------------------------------------------------------

* For the shake of simplicity:

- A in-memory database is used
- Only one user is created and used in the web frontend example (but as many users and animals as needed can be created calling the API)
- The frontend is just a long HTML+CSS+Javascript web page
- There are no Unit or Integration tests

* To apply the effects of the time passing to the animals' hunger and happiness, it's been decided that doing it when the user info is requested to the controller would be more efficient than having a background/daemon or scheduled process doing it periodically, as, first, it will sync exactly with the moment in which they're needed, and second, only the animals of a user actively playing the game would be updated, and not those of players that abandoned the game already, for instance, which will save processor time and resources.

* A CRUD set of basic methods have been implemented for both Users and Animals and then, extended the API with:
 - A method addAnimal() in UserController to add a new random animal to a user
 - Feed() and Pet() methods to AnimalController to perform the player actions