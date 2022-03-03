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
    private Dictionary<PaperType, int> spendablePaper;

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
        
    }
}
