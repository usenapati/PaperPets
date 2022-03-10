using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[JsonObject(MemberSerialization.Fields)]
public class WorldSim
{
    // current biome
    Biome biome;
    // is the world active
    bool isActive;
    // name of world
    public string name { get; private set; }
    // id of the world that will not change
    public string id { get; private set; }

    // mapping of organisms by name
    Dictionary<string, Species> organisms = new Dictionary<string, Species>();
    // mapping of organisms by tags
    Dictionary<string, List<Species>> speciesByTag = new Dictionary<string, List<Species>>();
    // mapping from tag to species that need to be alerted of changes. helps with efficiency of building a graph
    Dictionary<string, List<Species>> tagSubscribers = new Dictionary<string, List<Species>>();

    // debugging information
    Dictionary<string, StreamWriter> files = new Dictionary<string, StreamWriter>();

    public float availableLight { get; private set; } = 100;

    public WorldSim(string name)
    {
        this.name = name;
        this.isActive = false;
    }

    public List<KeyValuePair<string, int>> getAllSpeciesPopulation()
    {
        var species = new List<KeyValuePair<string, int>>();
        foreach (Species s in organisms.Values)
        {
            species.Add(new KeyValuePair<string, int>(s.name, s.population));
        }
        return species;
    }

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
    public void addSpecies(SpeciesType st)
    {
        Species s = new Species(st, this);

        organisms.Add(s.name, s);
        //files.Add(s.name, new StreamWriter(s.name + ".csv"));

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

    public string getPopulations()
    {
        string output = "";
        foreach (Species s in organisms.Values)
        {
            output += s.name + "\n";
            output += "   Population: " + s.population + "\n";
        }
        return output;
    }

    // updates the world and all organisms within
    public void updateWorld()
    {
        foreach (Species s in organisms.Values)
        {
            s.scheduleChanges();
        }

        foreach (Species s in organisms.Values)
        {
            s.update();
            //files[s.name].WriteLine(s.population);
            // update the paper amounts
            foreach (PaperValue p in s.producedPaper())
            {
                GameManager.Instance.GetSpendablePaper()[p.PaperColor] += p.PaperAmount;
            }
        }
    }

    public void closeFiles()
    {
        foreach (StreamWriter s in files.Values)
        {
            //s.Close();
        }
    }

}
