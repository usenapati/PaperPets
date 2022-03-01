using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species
{

    // species specific requirements
    public string name { get; private set; }
    float foodRequirements;
    float foodValue;
    float waterRequirements;
    float reproductionChance;

    // things species is, lives in, eats, and preferred biomes
    public HashSet<string> tags { get; private set; }
    HashSet<string> habitats;
    HashSet<string> foods;
    List<BiomeType> favoredBiomes;

    Dictionary<BiomeType, float> biomeWeights;

    // list of species this species is currently interacting with
    public List<Species> outgoingFood { get; private set; }
    public List<Species> outgoingHabitat { get; private set; }

    WorldSim world;

    public Species(string name, float foodReq, float foodVal, float waterReq, float repro, List<string> tags,
        List<string> habitats, List<string> foods, WorldSim world)
    {
        this.name = name; ;
        this.foodRequirements = foodReq;
        this.foodValue = foodVal;
        this.waterRequirements = waterReq;
        this.reproductionChance = repro;
        this.tags = new HashSet<string>();
        this.tags.UnionWith(tags);
        this.habitats = new HashSet<string>();
        this.habitats.UnionWith(habitats);
        this.foods = new HashSet<string>();
        this.foods.UnionWith(foods);
        this.world = world;

        outgoingFood = new List<Species>();
        outgoingHabitat = new List<Species>();

        var subscribeTo = new HashSet<string>();
        subscribeTo.UnionWith(habitats);
        subscribeTo.UnionWith(foods);
        world.subscribeToTags(this, subscribeTo);
        world.addSpecies(this);
    }

    public void notify(Species s)
    {
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

}
