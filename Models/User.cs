using System.Collections.Generic;
using System;

namespace pets.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Animal> Animals { get; set; }
        public void ModifyValuesFrom(User other){
            this.Name = other.Name;
            this.Animals = other.Animals;
        }
    }
}