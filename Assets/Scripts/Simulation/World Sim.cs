using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSim
{
    // current biome
    Biome biome;

    // mapping of organisms by name
    Dictionary<string, Species> organisms = new Dictionary<string, Species>();
    // mapping of organisms by tags
    Dictionary<string, List<Species>> speciesByTag = new Dictionary<string, List<Species>>();
    // mapping from tag to species that need to be alerted of changes. helps with efficiency of building a graph
    Dictionary<string, List<Species>> tagSubscribers = new Dictionary<string, List<Species>>();

    // gets all of the food organisms with any of the input tags
    public List<Species> getAllFoodByTag(List<string> tags)
    {
        List<Species> foodOrganisms = new List<Species>();
        foreach (string tag in tags)
        {
            if (speciesByTag.ContainsKey(tag))
                foodOrganisms.AddRange(speciesByTag[tag]);
        }
        return foodOrganisms;
    }

    // gets all of the habitat organisms with any of the input tags
    public List<Species> getAllHabitatsByTag(List<string> tags)
    {
        List<Species> habitatOrganisms = new List<Species>();
        foreach (string tag in tags)
        {
            if (speciesByTag.ContainsKey(tag))
                habitatOrganisms.AddRange(speciesByTag[tag]);
        }
        return habitatOrganisms;
    }

    // sets up a species to listen for other types of species being added
    public void subscribeToTags(Species s, IEnumerable<string> tags)
    {
        if (tags == null) return;

        // add the species for listening in the future
        foreach (string tag in tags)
        {
            if (!tagSubscribers.ContainsKey(tag))
                tagSubscribers.Add(tag, new List<Species>());
            tagSubscribers[tag].Add(s);
        }

        // get all of the things missed currently
        foreach (string tag in tags)
        {
            if (speciesByTag.ContainsKey(tag))
            {
                foreach (Species update in speciesByTag[tag])
                {
                    s.notify(update);
                }
            }
        }

    }

    // adds a new organism and updates all existing organisms
    public void addSpecies(Species s)
    {

        organisms.Add(s.name, s);

        // puts this species into the correct mapping of its own tags and updates subscribers
        foreach (string tag in s.tags)
        {
            if (!speciesByTag.ContainsKey(tag))
                speciesByTag.Add(tag, new List<Species>());
            speciesByTag[tag].Add(s);

            if (tagSubscribers.ContainsKey(tag))
            {
                foreach (Species sub in tagSubscribers[tag])
                {
                    sub.notify(s);
                }
            }
        }

    }

    public string getOutgoingFoods()
    {
        string output = "";
        foreach (Species s in organisms.Values)
        {
            output += s.name + "\n";
            foreach (Species f in s.outgoingFood)
            {
                output += "   " + f.name + "\n";
            }
        }
        return output;
    }

    public string getOutgoingHabitats()
    {
        string output = "";
        foreach (Species s in organisms.Values)
        {
            output += s.name + "\n";
            foreach (Species f in s.outgoingHabitat)
            {
                output += "   " + f.name + "\n";
            }
        }
        return output;
    }

}
