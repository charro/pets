using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pets.Models;
using System.Linq;
using System;

namespace pets.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly PetDBContext _context;
        private readonly AnimalsController _animalsController;
        public UsersController(PetDBContext context, AnimalsController animalsController)
        {
            _context = context;
            _animalsController = animalsController;

            if (_context.Users.Count() == 0)
            {
                User newUser = new User { Name = "Francis Bacon"};
                _context.Users.Add(newUser);
                _context.SaveChanges();
                // Add one animal to the user
                AddAnimalToUser(newUser.Id);
            }
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {   
            IEnumerable<User> userList = _context.Users.Include(u => u.Animals).ToList();
            return userList;
        }

        [HttpGet("{id}", Name = "GetUsers")]
        public IActionResult GetById(long id)
        {
            var user = _context.Users.Include(u => u.Animals).FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            
            bool modified = false;
            foreach(Animal animal in user.Animals){
                // Update the effects of time before to return the animals of the user
                if(animal.UpdateEffectsOfTime()){
                    _context.Animals.Update(animal);
                    modified = true;
                }
            }
            if(modified){
                _context.SaveChanges();
            }

            return new ObjectResult(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtRoute("GetUsers", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User user)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest();
            }

            User dbuser = _context.Users.FirstOrDefault(t => t.Id == id);
            if (dbuser == null)
            {
                return NotFound();
            }

            dbuser.ModifyValuesFrom(user);

            _context.Users.Update(dbuser);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return new NoContentResult();
        }

        /****************************************************  User's Animals related actions **********************************************************/

        // Add a new random animal to a user
        [HttpPost("{id}/addanimal")]
        public IActionResult AddAnimalToUser(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            Animal newAnimal = Animal.CreateRandom();
            newAnimal.UserId = user.Id;
            _context.Animals.Update(newAnimal);
            _context.SaveChanges();

            return CreatedAtRoute("GetUsers", new { id = user.Id }, user);
        }
    }
}