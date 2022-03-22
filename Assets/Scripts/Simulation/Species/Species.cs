using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[JsonObject(MemberSerialization.Fields)]
public class Species
{
    [JsonIgnore]
    SpeciesType type;

    // species specific requirements
    public string name { get; private set; }
    HashSet<string> foods;
    HashSet<string> habitats;
    public HashSet<string> tags { get; private set; }
    /*float foodRequirements;
    public float foodValue { get; private set; }
    float excessFoodRequired;
    float waterRequirements;
    float lightRequirements = 1;
    float reproductionChance;
    float maxReproduction;

    // things species is, lives in, eats, and preferred biomes
    bool requiresHabitat;
    bool requiresFood;
    List<BiomeType> favoredBiomes;

    Dictionary<BiomeType, float> biomeWeights;*/

    // list of species this species is currently interacting with
    public List<Species> outgoingFood { get; private set; }
    public List<Species> outgoingHabitat { get; private set; }
    public int population { get; private set; } = 0;

    WorldSim world;

    // things for updating the population
    [JsonProperty]
    bool needsUpdate = false;
    [JsonProperty]
    int populationToLose = 0;
    [JsonProperty]
    int populationToGain = 0;

    public Species(SpeciesType type, WorldSim world)
    {
        this.type = type;
        this.name = type.SpeciesName;
        /*this.foodRequirements = foodReq;
        this.foodValue = foodVal;
        this.waterRequirements = waterReq;
        this.reproductionChance = repro;*/
        this.tags = new HashSet<string>();
        this.tags.UnionWith(type.Tags);
        this.habitats = new HashSet<string>();
        this.habitats.UnionWith(type.Habitats);
        this.foods = new HashSet<string>();
        this.foods.UnionWith(type.Foods);
        this.world = world;

        outgoingFood = new List<Species>();
        outgoingHabitat = new List<Species>();
        population = 2;
        /*this.requiresHabitat = requiresHabitat;
        this.requiresFood = requiresFood;
        this.excessFoodRequired = excessFoodRequired;
        this.maxReproduction = maxReproduction;*/

        var subscribeTo = new HashSet<string>();
        subscribeTo.UnionWith(habitats);
        subscribeTo.UnionWith(foods);
        world.subscribeToTags(this, subscribeTo);
        //world.addSpecies(this);
    }

    public void notify(Species s)
    {
        if (s == this) return;
        HashSet<string> check = new HashSet<string>();
        // check for habitat
        check.UnionWith(habitats);
        check.IntersectWith(s.tags);
        if (check.Count > 0)
            outgoingHabitat.Add(s);

        // check for food
        check.Clear();
        check.UnionWith(foods);
        check.IntersectWith(s.tags);
        if (check.Count > 0)
            outgoingFood.Add(s);
    }

    public float getTotalFoodValue()
    {
        return type.FoodValue * population;
    }

    public void updateDecreaseDelta(int toLose)
    {
        populationToLose += toLose;
    }

    public void scheduleChanges()
    {
        bool canReproduce = true;
        float reproductionMultiplier = 1f;

        // checking food
        if (type.RequiresFood)
        {
            float reqIntake = population * type.FoodRequirements;

            //float totalPopulation = 0;
            float totalFoodAvailable = 0;
            foreach (Species s in outgoingFood)
            {
                //totalPopulation += s.population;
                totalFoodAvailable += s.getTotalFoodValue();
            }

            // more than enough food is available
            if (totalFoodAvailable >= reqIntake)
            {
                if (totalFoodAvailable < reqIntake * (1 + type.ExcessFoodRequired))
                {
                    canReproduce = false;
                    // try to implement boom and bust cycles
                    int death = Mathf.RoundToInt(population * Random.Range(.1f, .2f));
                    //Debug.Log("attempting to bust " + name);
                    updateDecreaseDelta(death);
                }
                else
                {
                    // a few random creatures die of age, accident, etc
                    int death = Mathf.RoundToInt(population * Random.Range(.005f, .01f));
                    updateDecreaseDelta(death);
                    reproductionMultiplier = 1 + totalFoodAvailable / reqIntake / type.ExcessFoodRequired;
                }

                foreach (Species s in outgoingFood)
                {
                    // see amount of food this organism makes up proportionally
                    float foodProportion = s.getTotalFoodValue() / totalFoodAvailable;
                    // food that should be taken from this species
                    float foodIntakeForSpecies = foodProportion * reqIntake;
                    // number of organisms that should be consumed
                    int toConsume = Mathf.RoundToInt(foodIntakeForSpecies / s.type.FoodValue);
                    s.updateDecreaseDelta(toConsume);
                }
            }
            // not enough food is available
            else
            {
                float percentFoodAvailable = totalFoodAvailable / reqIntake;
                foreach (Species s in outgoingFood)
                {
                    s.updateDecreaseDelta(s.population);
                }
                int death = Mathf.RoundToInt((reqIntake - totalFoodAvailable) / type.FoodRequirements);
                updateDecreaseDelta(death);
            }
        }
        // for plants
        else
        {
            if (population * type.LightRequirements >= world.availableLight) canReproduce = false;
            reproductionMultiplier = 1;
        }

        // checking habitat
        if (type.RequiresHabitat && outgoingHabitat.Count == 0)
        {
            canReproduce = false;
        }

        if (canReproduce)
        {
            if (type.RequiresFood) populationToGain += Mathf.RoundToInt(Mathf.Min(type.MaxReproduction, type.ReproductionChance * reproductionMultiplier));
            else if (type.ReproductionChance != 0) populationToGain += (int) Mathf.Min((world.availableLight - population * type.LightRequirements) / type.LightRequirements, population);
        }

        needsUpdate = true;
    }

    public void update()
    {
        if (!needsUpdate) throw new System.Exception("Species '" + name + "' has no scheduled updates.");
        needsUpdate = false;

        population = Mathf.Max(0, population - populationToLose);
        if (population > 1 && populationToLose == 0 && Random.Range(0, 100) < 1)
            population += 1;
        populationToLose = 0;
        population += populationToGain;
        populationToGain = 0;

        if (population == 0)
        {
            // species goes extinct
            world.killSpecies(this);
        }
    }

    // returns the list of paper this species produced
    public List<PaperValue> producedPaper()
    {
        List<PaperValue> paper = new List<PaperValue>();
        foreach (PaperValue pv in type.SpeciesProduce)
        {
            paper.Add(new PaperValue(pv.PaperColor, population * pv.PaperAmount));
        }
        return paper;
    }

    public void resetSpecies()
    {
        type = Resources.Load("Species/" + name) as SpeciesType;
    }

}
