using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrariumContainerManager : MonoBehaviour
{

    [SerializeField] GameObject terrariumButtonPrefab;

    Vector3 translation = new Vector3();

    // Start is called before the first frame update
    void Start()
    {

        foreach (KeyValuePair<string, string> kv in GameManager.Instance.GetWorldIDs())
        {
            GameObject temp = Instantiate(terrariumButtonPrefab, transform);
            temp.transform.Translate(translation);
            translation.x += temp.GetComponent<RectTransform>().rect.width + 50;
            temp.GetComponent<TerrariumSelector>().SetTerrariumData(kv.Key, kv.Value);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTerrarium(string name)
    {
        string id = GameManager.Instance.CreateWorld(name);
        GameObject temp = Instantiate(terrariumButtonPrefab, transform);
        temp.transform.Translate(translation);
        translation.x += temp.GetComponent<RectTransform>().rect.width + 50;
        temp.GetComponent<TerrariumSelector>().SetTerrariumData(id, name);
    }

}
