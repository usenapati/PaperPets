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
    [SerializeField] GameObject SpeciesCircle;
    [SerializeField] Canvas canvas;

    private void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        RectTransform circleSize = SpeciesCircle.GetComponent<RectTransform>();

        string newtext = "";
        totalPop = 0;
        foreach (KeyValuePair<string, int> kv in GameManager.Instance.getCurrentWorld().getAllSpeciesPopulation())
        {
            //newtext += kv.Key + ": " + kv.Value + "\n";
            totalPop += kv.Value;
            GameObject s = Instantiate(SpeciesCircle, new Vector3(Random.Range(circleSize.rect.width, rect.rect.width - circleSize.rect.width), Random.Range(circleSize.rect.height, rect.rect.height - circleSize.rect.height), 0), new Quaternion(), canvas.transform);
            s.transform.localScale = new Vector3(2, 2, 1);
            s.transform.Find("Species").GetComponent<TextMeshProUGUI>().SetText(kv.Key);
            s.transform.Find("Population").GetComponent<TextMeshProUGUI>().SetText(kv.Value.ToString());
        }

        species.SetText(newtext);
        pop.SetText(totalPop.ToString());
        print("test");
    }

}
