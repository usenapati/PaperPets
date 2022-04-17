using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[JsonObject(MemberSerialization.Fields)]
public class WorldSim
{
    // current biome
    Biome biome = new Biome(0, 0, 0);
    // is the world active
    bool isActive;
    // name of world
    public string name { get; private set; }
    // id of the world that will not change
    public string id { get; private set; }

    // mapping of organisms by name
    Dictionary<string, Species> organisms = new Dictionary<string, Species>();
    // mapping of organisms by tags
    Dictionary<string, HashSet<Species>> speciesByTag = new Dictionary<string, HashSet<Species>>();
    // mapping from tag to species that need to be alerted of changes. helps with efficiency of building a graph
    Dictionary<string, HashSet<Species>> tagSubscribers = new Dictionary<string, HashSet<Species>>();

    // list of species that have actively died and need to be removed
    HashSet<Species> toBeRemoved = new HashSet<Species>();

    // debugging information
    Dictionary<string, StreamWriter> files = new Dictionary<string, StreamWriter>();

    // light information
    private int lightUpgradeLevel = 1;
    private float baseLight = 50f;
    public float availableLight { get { return baseLight * lightUpgradeLevel; } private set { } }

    // water information
    private int waterUpgradeLevel = 1;
    private float baseWater = 100f;
    public float availableWaterPerSpecies { get { return baseWater * waterUpgradeLevel / organisms.Count; } private set { } }
    public float availableTotalWater { get { return baseWater * waterUpgradeLevel; } private set { } }

    public WorldSim(string name)
    {
        this.name = name;
        this.isActive = false;
    }

    #region Statistic Getters
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

    public List<Species> getAllSpecies()
    {
        return new List<Species>(organisms.Values);
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

    public Biome getBiome()
    {
        return biome;
    }
    #endregion

    #region Species Manipulation
    // sets up a species to listen for other types of species being added
    public void subscribeToTags(Species s, IEnumerable<string> tags)
    {
        if (tags == null) return;

        // add the species for listening in the future
        foreach (string tag in tags)
        {
            if (!tagSubscribers.ContainsKey(tag))
                tagSubscribers.Add(tag, new HashSet<Species>());
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
                speciesByTag.Add(tag, new HashSet<Species>());
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

    // removes all species scheduled to be killed
    private void killAllScheduledSpecies()
    {
        foreach (Species s in toBeRemoved)
        {
            foreach (string tag in s.tags)
            {
                speciesByTag[tag].Remove(s);
                if (tagSubscribers.ContainsKey(tag))
                {
                    foreach (Species sub in tagSubscribers[tag])
                    {
                        sub.outgoingFood.Remove(s);
                        sub.outgoingHabitat.Remove(s);
                    }
                }
            }
            foreach (HashSet<Species> h in tagSubscribers.Values)
            {
                h.Remove(s);
            }
            organisms.Remove(s.name);
        }
        toBeRemoved.Clear();
    }

    // species is killed and all subscribers need to be notified
    public void killSpecies(Species s)
    {
        toBeRemoved.Add(s);
    }
    #endregion

    #region Other Manipulators
    private (float, float, float, float) recalculateBiome(Species s)
    {
        return s.getBiomeInfo();
    }

    public int getWaterLevel()
    {
        return waterUpgradeLevel;
    }

    public int getLightLevel()
    {
        return lightUpgradeLevel;
    }

    public void upgradeLightLevel()
    {
        lightUpgradeLevel++;
    }

    public void upgradeWaterLevel()
    {
        waterUpgradeLevel++;
    }
    #endregion

    // updates the world and all organisms within
    public void updateWorld()
    {
        foreach (Species s in organisms.Values)
        {
            s.scheduleChanges();
        }

        (float, float, float, float) biomeData = (0, 0, 0, 1);
        foreach (Species s in organisms.Values)
        {
            s.update();
            //files[s.name].WriteLine(s.population);
            // update the paper amounts
            foreach (PaperValue p in s.producedPaper())
            {
                GameManager.Instance.GetSpendablePaper()[p.PaperColor] += (int) p.PaperAmount;
            }
            (float, float, float, float) speciesInfluence = recalculateBiome(s);
            biomeData.Item1 += speciesInfluence.Item1;
            biomeData.Item2 += speciesInfluence.Item2;
            biomeData.Item3 += speciesInfluence.Item3;
            biomeData.Item4 += speciesInfluence.Item4;
        }
        biome.updateBiome(biomeData);
        //Debug.Log(biome);
        killAllScheduledSpecies();
    }

    public void onLoadIn()
    {
        foreach (Species s in organisms.Values)
        {
            s.resetSpecies();
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
