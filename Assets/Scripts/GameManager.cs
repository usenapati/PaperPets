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

    // Information being kept
    // The paper the player has available to spend
    private Dictionary<PaperType, int> spendablePaper;
    // The simulation time tick
    private const float dt = 0.2f;
    private float accumulator = 0f;
    private uint tick;


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
    }

    public Dictionary<PaperType, int> GetSpendablePaper()
    {
        return spendablePaper;
    }

    // Update is called once per frame
    void Update()
    {
        // Hours, Days, Months, Half-year, Years
        accumulator += Time.deltaTime;
        if (accumulator >= dt)
        {
            accumulator = 0;
            tick++;

            // A new tick has passed
            // Do we want to tie animations to this tick or have it based on something else?
        }
    }
}
