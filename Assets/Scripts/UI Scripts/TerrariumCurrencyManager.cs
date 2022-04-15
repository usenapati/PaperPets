using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrariumCurrencyManager : MonoBehaviour
{

    Dictionary<PaperType, int> paperamounts;
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
        foreach (KeyValuePair<PaperType, int> kv in paperamounts)
        {
            switch (kv.Key.PaperName)
            {
                case "green":
                    green.text = kv.Value.ToString();
                    break;
                case "blue":
                    blue.text = kv.Value.ToString();
                    break;
                case "yellow":
                    yellow.text = kv.Value.ToString();
                    break;
                case "orange":
                    orange.text = kv.Value.ToString();
                    break;
                case "brown":
                    brown.text = kv.Value.ToString();
                    break;
                case "white":
                    white.text = kv.Value.ToString();
                    break;
                case "red":
                    red.text = kv.Value.ToString();
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
