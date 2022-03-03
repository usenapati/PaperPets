using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesBuilder : MonoBehaviour
{

    public string speciesname;
    public float foodRequirements;
    public float foodValue;
    public float excessFoodRequired;
    public float waterRequirements;
    public float reproductionChance;
    public float maxReproduction;
    public bool requiresHabitat;
    public bool requiresFood;
    public List<string> tags = new List<string>();
    public List<string> habitats = new List<string>();
    public List<string> foods = new List<string>();
    public WorldRunner world;

    public Species s;

    // Start is called before the first frame update
    void Start()
    {
        s = new Species(speciesname, foodRequirements, foodValue, waterRequirements, reproductionChance, tags, habitats, foods, world.world, requiresHabitat, requiresFood, excessFoodRequired, maxReproduction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
