using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemView : MonoBehaviour
{    
    public TextMeshProUGUI pop;
    public TextMeshProUGUI species;
    private int totalPop;

    private void Start()
    {
        

        string newtext = "";
        totalPop = 0;
        foreach (KeyValuePair<string, int> kv in GameManager.Instance.getCurrentWorld().getAllSpeciesPopulation())
        {
            newtext += kv.Key + ": " + kv.Value + "\n";
            totalPop += kv.Value;
        }

        species.SetText(newtext);
        pop.SetText(totalPop.ToString());
        print("test");
    }

}
