using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using pets.Models;
using System.Linq;
using System;

namespace pets.Controllers
{
    [Route("api/[controller]")]
    public class AnimalsController : Controller
    {
        private readonly PetDBContext _context;

        public AnimalsController(PetDBContext context)
        {
            _context = context;

            if (_context.Animals.Count() == 0)
            {
                _context.Animals.Add(Animal.CreateRandom());
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Animal> GetAll()
        {   
            bool modified = false;

            IEnumerable<Animal> animalList = _context.Animals.ToList();
            foreach(Animal animal in animalList){
                // Update the effects of time before to return it
                if(animal.UpdateEffectsOfTime()){
                    _context.Animals.Update(animal);
                    modified = true;
                }
            }
            if(modified){
                _context.SaveChanges();
            }
            return animalList;
        }

        [HttpGet("{id}", Name = "GetAnimals")]
        public IActionResult GetById(long id)
        {
            var animal = _context.Animals.FirstOrDefault(t => t.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            // Update the effects of time before to return it
            animal.UpdateEffectsOfTime();
            _context.Animals.Update(animal);
            _context.SaveChanges();
            
            return new ObjectResult(animal);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Animal animal)
        {
            if (animal == null)
            {
                return BadRequest();
            }

            _context.Animals.Add(animal);
            _context.SaveChanges();

            return CreatedAtRoute("GetAnimals", new { id = animal.Id }, animal);
        }

        [HttpPost("random")]
        public IActionResult CreateRandom()
        {
            Animal randomAnimal = Animal.CreateRandom();

            _context.Animals.Add(randomAnimal);
            _context.SaveChanges();

            return CreatedAtRoute("GetAnimals", new { id = randomAnimal.Id }, randomAnimal);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Animal animal)
        {
            if (animal == null || animal.Id != id)
            {
                return BadRequest();
            }

            Animal dbanimal = _context.Animals.FirstOrDefault(t => t.Id == id);
            if (dbanimal == null)
            {
                return NotFound();
            }

            dbanimal.ModifyValuesFrom(animal);

            _context.Animals.Update(dbanimal);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var animal = _context.Animals.FirstOrDefault(t => t.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpPost("{id}/pet")]
        public IActionResult Pet(long id)
        {
            var animal = _context.Animals.FirstOrDefault(t => t.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            animal.Pet();   // Apply all the effects of pet to the animal
            _context.Animals.Update(animal);
            _context.SaveChanges();
              // The animal is modified after this action so the updated info is returned
            return new CreatedAtRouteResult("GetAnimals", new { id = animal.Id }, animal);
        }

        [HttpPost("{id}/feed")]
        public IActionResult Feed(long id)
        {
            var animal = _context.Animals.FirstOrDefault(t => t.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            animal.Feed();   // Apply all the effects of feeding to the animal
            _context.Animals.Update(animal);
            _context.SaveChanges();
              // The animal is modified after this action so the updated info is returned
            return new CreatedAtRouteResult("GetAnimals", new { id = animal.Id }, animal); 
        }
    }
}