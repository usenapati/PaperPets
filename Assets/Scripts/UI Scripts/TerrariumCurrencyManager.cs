using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrariumCurrencyManager : MonoBehaviour
{

    Dictionary<PaperType, float> paperamounts;
    [SerializeField] Text green;
    [SerializeField] Text blue;
    [SerializeField] Text yellow;
    [SerializeField] Text orange;
    [SerializeField] Text brown;
    [SerializeField] Text white;
    [SerializeField] Text red;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.pause();
        paperamounts = GameManager.Instance.GetSpendablePaper();
        foreach (KeyValuePair<PaperType, float> kv in paperamounts)
        {
            switch (kv.Key.PaperName)
            {
                case "green":
                    green.text = ((int)kv.Value).ToString();
                    break;
                case "blue":
                    blue.text = ((int)kv.Value).ToString();
                    break;
                case "yellow":
                    yellow.text = ((int)kv.Value).ToString();
                    break;
                case "orange":
                    orange.text = ((int)kv.Value).ToString();
                    break;
                case "brown":
                    brown.text = ((int)kv.Value).ToString();
                    break;
                case "white":
                    white.text = ((int)kv.Value).ToString();
                    break;
                case "red":
                    red.text = ((int)kv.Value).ToString();
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
