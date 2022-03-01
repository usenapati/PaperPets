using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesBuilder : MonoBehaviour
{

    public string speciesname;
    public float foodRequirements;
    public float foodValue;
    public float waterRequirements;
    public float reproductionChance;
    public List<string> tags = new List<string>();
    public List<string> habitats = new List<string>();
    public List<string> foods = new List<string>();
    public WorldRunner world;

    public Species s;

    // Start is called before the first frame update
    void Start()
    {
        s = new Species(name, foodRequirements, foodValue, waterRequirements, reproductionChance, tags, habitats, foods, world.world);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
