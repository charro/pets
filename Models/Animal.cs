using System;

namespace pets.Models
{
    public class Animal
    {
        public static int MAX_HAPPINESS = 100;
        public static int MIN_HAPPINESS = -100;
        public static int MAX_HUNGER = 100;
        public static int MIN_HUNGER = -100;

        public Animal(){
            LastHappinessUpdate = DateTime.Now;
            LastHungerUpdate = DateTime.Now;
            Happiness = 0;
            Hunger = 0;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int Happiness { get; set; }
        public int Hunger { get; set; }
        public AnimalType Type { get; set; }
        public DateTime LastHappinessUpdate { get; set; }
        public DateTime LastHungerUpdate { get; set; }
        public void Pet(){
            AddHappiness(Config.Config.PET_HAPPINESS_INCREASE_AMOUNT);
        }
        public void Feed(){
            AddHunger(Config.Config.FEED_HUNGER_DECREASE_AMOUNT);
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
        public void UpdateEffectsOfTime(){
            long secondsPassedSinceLastPet = (long)(DateTime.Now - LastHappinessUpdate).TotalSeconds;
            if(secondsPassedSinceLastPet > Config.Config.SECONDS_TO_LOSE_HAPPINESS){
                int happinessLost = (int) secondsPassedSinceLastPet * Config.Config.NO_PET_HAPPINESS_DECREASE_AMOUNT;
                AddHappiness(-happinessLost);
            }

            long secondsPassedSinceLastFeed = (long)(DateTime.Now - LastHungerUpdate).TotalSeconds;
            if(secondsPassedSinceLastFeed > Config.Config.SECONDS_TO_INCREASE_HUNGER){
                int hungerIncreased = (int) secondsPassedSinceLastFeed * Config.Config.NO_FEED_HUNGER_INCREASE_AMOUNT;
                AddHunger(hungerIncreased);
            }
        }
    }
}