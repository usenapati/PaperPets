using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class DisplayPaperTemp : MonoBehaviour
{
    //public Text text;
    private Dictionary<PaperType, float> paperamounts;

    private void Start()
    {
        paperamounts = GameManager.Instance.GetSpendablePaper();
    }

    // Update is called once per frame
    void Update()
    {
        string newtext = "";
        foreach (KeyValuePair<PaperType, float> kv in paperamounts)
        {
            newtext += kv.Key.name + ": " + kv.Value + "\n";
        }

        //text.text = newtext;
    }
}
