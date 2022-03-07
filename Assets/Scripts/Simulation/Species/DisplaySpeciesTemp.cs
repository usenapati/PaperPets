using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySpeciesTemp : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string newtext = "";
        foreach (KeyValuePair<string, int> kv in GameManager.Instance.getCurrentWorld().getAllSpeciesPopulation())
        {
            newtext += kv.Key + ": " + kv.Value + "\n";
        }

        text.text = newtext;
    }
}
