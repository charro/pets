using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using pets.Models;
using System.Linq;
using System;

namespace pets.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly PetDBContext _context;

        public UsersController(PetDBContext context)
        {
            _context = context;

            if (_context.Users.Count() == 0)
            {
                _context.Users.Add(new User { Name = "Francis Bacon", Animals = new List<Animal>() });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {   
            IEnumerable<User> userList = _context.Users.ToList();
            return userList;
        }

        [HttpGet("{id}", Name = "GetUsers")]
        public IActionResult GetById(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Update(user);
            _context.SaveChanges();
            
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
    }
}