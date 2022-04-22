using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Based off of the article from https://csharpindepth.com/Articles/Singleton#dcl
 * Game manager be singleton holding important information
 */
public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    private static readonly object padlock = new object();

    // Set up a a place for other tics to be notified by game manager
    private Dictionary<string, UnityEvent<float>> m_TickEvents;

    // Set up the terrariums
    // will create ID later for the terrariums
    private Dictionary<string, WorldSim> terrariums;
    private int nextID = 0;
    private string mainWorld;
    // Information being kept
    // The paper the player has available to spend
    // Look through specific file path to find all types of paper
    private Dictionary<PaperType, int> spendablePaper;
    private ProgressionSystem progressionSystem;
    private string filename;

    // The simulation time tick default
    public float dt = 1f;
    private const float basedt = 1f;
    // Will divide dt by TIMESPEED
    public enum TIMESPEED { SLOWEST = 1, SLOW, NORMAL, FAST, FASTEST }
    private TIMESPEED timeSpeed = TIMESPEED.NORMAL;
    private float accumulator = 0f;
    private uint tick = 0;
    private bool paused = false;
    private float test;

    private Dictionary<string, bool> isOwned = new Dictionary<string, bool>();



    // Get the instance of the game manager
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            _instance.paused = false;
            return;
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);

        filename = "BasicSave";

        // Set up the tick events
        m_TickEvents = new Dictionary<string, UnityEvent<float>>();

        // Prepare the dictionary for the paper currencies
        spendablePaper = new Dictionary<PaperType, int>();

        // Prepare progression system
        progressionSystem = new ProgressionSystem();
        progressionSystem.setup();

        // load the paper dictionary
        foreach (PaperType p in Resources.FindObjectsOfTypeAll(typeof(PaperType)) as PaperType[])
        {
            spendablePaper.Add(p, 0);
        }

        // Prepare the dictionary for terrariums
        terrariums = new Dictionary<string, WorldSim>();
        mainWorld = nextID++.ToString();
        terrariums.Add(mainWorld, new WorldSim("First World"));
    }

    public void LoadGame(string filename)
    {
        SaveSystem.SaveData temp = SaveSystem.SavesManager.LoadGame(filename);
        terrariums = temp.GetTerrariums();
        spendablePaper = temp.GetSpendablePaper();
        progressionSystem = temp.GetProgressionSystem();
        isOwned = temp.GetOwnedDictionary();
        nextID = terrariums.Count;
        
        foreach (string s in progressionSystem.getUnlocks())
        {
            Debug.Log(s);
        }

    }

    public void SaveGame(string filename)
    {
        SaveSystem.SavesManager.SaveGame(filename,
            new SaveSystem.SaveData(terrariums, spendablePaper, progressionSystem, isOwned));
    }


    public Dictionary<PaperType, int> GetSpendablePaper()
    {
        return spendablePaper;
    }

    public void SetSpendablePaper(Dictionary<PaperType, int> s)
    {
        spendablePaper = s;
    }

    public void SetTimeSpeed(TIMESPEED tSpeed)
    {
        accumulator *= (float)timeSpeed / (float)tSpeed;
        timeSpeed = tSpeed;
        dt = basedt / (float)timeSpeed;
    }

    public void addSpecies(SpeciesType s)
    {
        terrariums[mainWorld].addSpecies(s);
    }

    public WorldSim getCurrentWorld()
    {
        return terrariums[mainWorld];
    }
    
    public void SwitchCurrentWorld(string id)
    {
        if (terrariums.ContainsKey(id))
        {
            mainWorld = id;
        }
        else
        {
            Debug.LogWarning("Attempted to switch current world to non existant world with key: " + id);
        }
    }

    public string CreateWorld(string name)
    {
        if (terrariums.ContainsKey((nextID).ToString())) return null;
        string newID = nextID++.ToString();
        terrariums.Add(newID, new WorldSim(name));
        return newID;
    }

    public Dictionary<string, string> GetWorldIDs()
    {
        Dictionary<string, string> ids = new Dictionary<string, string>();
        foreach (KeyValuePair<string, WorldSim> kv in terrariums)
        {
            ids.Add(kv.Key, kv.Value.name);
        }
        return ids;
    }

    public ProgressionSystem getProgression()
    {
        return progressionSystem;
    }

    public void addUnityEvent(string name, UnityEvent<float> unityEvent)
    {
        m_TickEvents.Add(name, unityEvent);
    }

    public UnityEvent<float> getUnityTickEvent(string name)
    {
        return m_TickEvents.ContainsKey(name) ? m_TickEvents[name] : null;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) return;
        accumulator += Time.deltaTime;
        if (accumulator >= dt)
        {
            tick++;
            accumulator = 0;
            //Debug.Log("tick" + timeSpeed.ToString() + ":" + tick);

            // check progression
            progressionSystem.checkUnlocks();
            m_TickEvents[DayNightCycle.tickEventName].Invoke(dt);

            // A new tick has passed
            // Do we want to tie animations to this tick or have it based on something else?
            foreach (WorldSim terrarium in terrariums.Values)
            {
                terrarium.updateWorld();
            }
        }
        
    }

    public void waterUpgrade()
    {
        getCurrentWorld().upgradeWaterLevel();
    }

    public void lightUpgrade()
    {
        getCurrentWorld().upgradeLightLevel();
    }

    public int getWaterCost()
    {
        return (int) 50 * getCurrentWorld().getWaterLevel();
    }

    public int getLightCost()
    {
        return (int) 50 * getCurrentWorld().getLightLevel();
    }

    public Biome getCurrentBiome()
    {
        return getCurrentWorld().getBiome();
    }

    public void setOwned(Dictionary<string, bool> owned)
    {
        isOwned = owned;
    }

    public Dictionary<string, bool> getOwned()
    {
        return isOwned;
    }

    public void pause()
    {
        paused = true;
    }

    public float getUIZoom()
    {
        return test;
    }

    public void setUIZoom(float zoom)
    {
        test = zoom;
    }

}
