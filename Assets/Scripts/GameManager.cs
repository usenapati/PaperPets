using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Based off of the article from https://csharpindepth.com/Articles/Singleton#dcl
 * Game manager be singleton holding important information
 */
public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager _instance;
    private static readonly object padlock = new object();

    // Set up the terrariums
    // will create ID later for the terrariums
    private Dictionary<string, WorldSim> terrariums;
    private int nextID = 0;
    // Information being kept
    // The paper the player has available to spend
    // Look through specific file path to find all types of paper
    private Dictionary<PaperType, int> spendablePaper;
    private string filename;

    // The simulation time tick default
    private float dt = 1f;
    private const float basedt = 1f;
    // Will divide dt by TIMESPEED
    public enum TIMESPEED { SLOWEST = 1, SLOW, NORMAL, FAST, FASTEST }
    private TIMESPEED timeSpeed = TIMESPEED.NORMAL;
    private float accumulator = 0f;
    private uint tick = 0;



    // Get the instance of the game manager
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);

        filename = "BasicSave";

        // Prepare the dictionary for the paper currencies
        spendablePaper = new Dictionary<PaperType, int>();

        // load the paper dictionary
        foreach (PaperType p in Resources.FindObjectsOfTypeAll(typeof(PaperType)) as PaperType[])
        {
            spendablePaper.Add(p, 0);
        }

        // Prepare the dictionary for terrariums
        terrariums = new Dictionary<string, WorldSim>();
        terrariums.Add(nextID++.ToString(), new WorldSim("first world"));
        SetTimeSpeed(this.timeSpeed);
    }

    public void LoadGame(string filename)
    {
        SaveSystem.SaveData temp = SaveSystem.SavesManager.LoadGame(filename);
        terrariums = temp.GetTerrariums();
        spendablePaper = temp.GetSpendablePaper();
    }

    public void SaveGame(string filename)
    {
        SaveSystem.SavesManager.SaveGame(filename,
            new SaveSystem.SaveData(terrariums, spendablePaper));
    }


    public Dictionary<PaperType, int> GetSpendablePaper()
    {
        return spendablePaper;
    }

    public void SetTimeSpeed(TIMESPEED tSpeed)
    {
        accumulator *= (float)timeSpeed / (float)tSpeed;
        timeSpeed = tSpeed;
        dt = basedt / (float)timeSpeed;
    }

    public void addSpecies(SpeciesType s)
    {
        terrariums[(nextID - 1).ToString()].addSpecies(s);
    }

    public WorldSim getCurrentWorld()
    {
        return terrariums[(nextID - 1).ToString()];
    }

    // Update is called once per frame
    void Update()
    {
        accumulator += Time.deltaTime;
        if (accumulator >= dt)
        {
            tick++;
            accumulator = 0;
            //Debug.Log("tick" + timeSpeed.ToString() + ":" + tick);

            // A new tick has passed
            // Do we want to tie animations to this tick or have it based on something else?
            foreach (WorldSim terrarium in terrariums.Values)
            {
                terrarium.updateWorld();
            }
        }
    }
}
