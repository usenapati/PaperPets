using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemView : MonoBehaviour
{    
    private int totalPop;
    [SerializeField] TextMeshProUGUI pop;
    [SerializeField] Renderer rect;
    [SerializeField] GameObject SpeciesCircle;
    [SerializeField] float startingScale;

    private void Start()
    {
        Vector3 dim = rect.bounds.size;
        Vector3 circleSize = SpeciesCircle.GetComponent<Renderer>().bounds.size;

        totalPop = 0;
        foreach (KeyValuePair<string, int> kv in GameManager.Instance.getCurrentWorld().getAllSpeciesPopulation())
        {
            totalPop += kv.Value;
            GameObject s = Instantiate(SpeciesCircle, new Vector3(Random.Range(circleSize.x, dim.x - circleSize.x), -1, Random.Range(circleSize.z, dim.z - circleSize.z)), new Quaternion(), transform);
            s.transform.localScale = new Vector3(startingScale, startingScale, 1);
            s.transform.position += new Vector3(dim.x / -2, 0, dim.z / -2);
            s.transform.position += new Vector3(0, 0, 1);
            s.transform.Find("Species").GetComponent<TextMeshPro>().SetText(kv.Key + "\n" + kv.Value);
        }

        pop.SetText(totalPop.ToString());
        print("test");
    }

}
