using System;
using System.ComponentModel.DataAnnotations;

namespace pets.Models
{
    public class Animal
    {
        // Model properties
        public static int MAX_HAPPINESS = 100;
        public static int MIN_HAPPINESS = -100;
        public static int MAX_HUNGER = 100;
        public static int MIN_HUNGER = -100;
        public long Id { get; set; }
        public string Name { get; set; }
        public int Happiness { get; set; }
        public int Hunger { get; set; }
        public AnimalType Type { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastHappinessUpdate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LastHungerUpdate { get; set; }
        
        // DB Foreign Key = User this animal belongs to
        public long UserId { get; set; }
       // public User User { get; set; }

        // Utility methods
        public void Pet(){
            AddHappiness(Config.Config.PET_HAPPINESS_INCREASE_AMOUNT);
        }
        public void Feed(){
            AddHunger(-Config.Config.FEED_HUNGER_DECREASE_AMOUNT);
        }
        private void AddHappiness(int happinessAmount) {
            Happiness += happinessAmount;

            Happiness = Math.Min(Happiness, MAX_HAPPINESS);     // Can't be more than MAX
            Happiness = Math.Max(Happiness, MIN_HAPPINESS);     // Can't be less than MIN

            LastHappinessUpdate = DateTime.Now;
        }
        private void AddHunger(int hungerAmount) {
            Hunger += hungerAmount;

            Hunger = Math.Min(Hunger, MAX_HUNGER);     // Can't be more than MAX
            Hunger = Math.Max(Hunger, MIN_HUNGER);     // Can't be less than MIN
        
            LastHungerUpdate  = DateTime.Now;
        }
        public void ModifyValuesFrom(Animal other){
            this.Name = other.Name;
            this.Happiness = other.Happiness;
            this.Hunger = other.Hunger;
            this.Type = other.Type;
            this.LastHappinessUpdate = other.LastHappinessUpdate;
            this.LastHungerUpdate = other.LastHungerUpdate;
        }
        public bool UpdateEffectsOfTime(){
            bool modified = false;

            // The type acts as a multiplier for the time
            int secondsToLoseHappiness = (int)this.Type * Config.Config.SECONDS_TO_LOSE_HAPPINESS;
            double secondsPassedSinceLastHappinessUpdate = (DateTime.Now - LastHappinessUpdate).TotalSeconds;
            if(secondsPassedSinceLastHappinessUpdate >= secondsToLoseHappiness){
                int happinessLost = 
                    (int) (secondsPassedSinceLastHappinessUpdate / secondsToLoseHappiness) * Config.Config.NO_PET_HAPPINESS_DECREASE_AMOUNT;
                AddHappiness(-happinessLost);
                modified = true;
            }

            int secondsToIncreaseHunger = (int)this.Type * Config.Config.SECONDS_TO_INCREASE_HUNGER;
            double secondsPassedSinceLastHungerUpdate = (DateTime.Now - LastHungerUpdate).TotalSeconds;
            if(secondsPassedSinceLastHungerUpdate >= secondsToIncreaseHunger){
                int hungerIncreased = 
                    (int) (secondsPassedSinceLastHungerUpdate / secondsToIncreaseHunger) * Config.Config.NO_FEED_HUNGER_INCREASE_AMOUNT;
                AddHunger(hungerIncreased);
                modified = true;
            }

            return modified;
        }
        // Factory of random animals
        public static Animal CreateRandom(){
            String[] adjectives = {"Pretty", "Cutey", "Happy", "Tiny", "Nicey"};
            Array values = Enum.GetValues(typeof(AnimalType));
            Random random = new Random();

            Animal newAnimal = new Animal();
            newAnimal.Type = (AnimalType)values.GetValue(random.Next(values.Length));
            newAnimal.Name = adjectives[random.Next(adjectives.Length)] + " " + Enum.GetName(typeof(AnimalType), newAnimal.Type);
            newAnimal.Happiness = 0;
            newAnimal.Hunger = 0;
            newAnimal.LastHappinessUpdate = DateTime.Now;
            newAnimal.LastHungerUpdate = DateTime.Now;

            return newAnimal;
        }
    }
}