using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using pets.Models;
using System.Linq;

namespace pets.Controllers
{
    [Route("api/[controller]")]
    public class AnimalsController : Controller
    {
        private readonly AnimalsContext _context;

        public AnimalsController(AnimalsContext context)
        {
            _context = context;

            if (_context.Animals.Count() == 0)
            {
                _context.Animals.Add(new Animal { Name = "New Animal" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Animal> GetAll()
        {
            IEnumerable<Animal> animalList = _context.Animals.ToList();
            foreach(Animal animal in animalList){
                animal.UpdateEffectsOfTime();
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

            animal.UpdateEffectsOfTime();
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

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Animal animal)
        {
            if (animal == null || animal.Id != id)
            {
                return BadRequest();
            }

            animal = _context.Animals.FirstOrDefault(t => t.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            animal.ModifyValuesFrom(animal);

            _context.Animals.Update(animal);
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