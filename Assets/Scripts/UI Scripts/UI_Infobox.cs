using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI; 

public class UI_Infobox : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPointerEnter()
    {
        GameObject[] temp;
        temp = GameObject.FindGameObjectsWithTag("infoimage");
        foreach(GameObject g in temp)
        {
            g.GetComponent<RawImage>().enabled = true;
        }
        temp = GameObject.FindGameObjectsWithTag("infotext");
        foreach(GameObject g in temp)
        {
            g.GetComponent<TextMeshProUGUI>().enabled = true;
        }
        temp = GameObject.FindGameObjectsWithTag("infoimage2");
        foreach(GameObject g in temp)
        {
            g.GetComponent<Image>().enabled = true;
        }
        
        //print("enter");
    }

    public void OnPointerExit()
    {
        GameObject[] temp;
        temp = GameObject.FindGameObjectsWithTag("infoimage");
        foreach(GameObject g in temp)
        {
            g.GetComponent<RawImage>().enabled = false;
        }
        temp = GameObject.FindGameObjectsWithTag("infotext");
        foreach(GameObject g in temp)
        {
            g.GetComponent<TextMeshProUGUI>().enabled = false;
        }
        temp = GameObject.FindGameObjectsWithTag("infoimage2");
        foreach(GameObject g in temp)
        {
            g.GetComponent<Image>().enabled = false;
        }
        //print("exit");
    }
}
