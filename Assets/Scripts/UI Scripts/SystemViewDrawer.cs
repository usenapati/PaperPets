using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemViewDrawer : MonoBehaviour
{

    [SerializeField] GameObject button;
    [SerializeField] GameObject minimap;
    [SerializeField] GameObject arrow;
    bool expanded = true;

    public void Toggle()
    {
        if (expanded)
        {
            minimap.SetActive(false);
            button.transform.Translate(new Vector3(0, -minimap.GetComponent<RectTransform>().rect.height, 0));
        }
        else
        {
            minimap.SetActive(true);
            button.transform.Translate(new Vector3(0, minimap.GetComponent<RectTransform>().rect.height, 0));
        }
        expanded = !expanded;
        arrow.transform.Rotate(new Vector3(180, 0, 0));
    }
}
