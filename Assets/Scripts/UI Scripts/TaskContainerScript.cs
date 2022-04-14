using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskContainerScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        RectTransform rt;
        RectTransform bgsize;
        Vector3 pos = new Vector3();
        //pos.x = transform.position.x;
        for (int i = 0; i < transform.childCount; i++)
        {
            rt = transform.GetChild(i).GetComponent<RectTransform>();
            bgsize = transform.GetChild(i).Find("unlockTemplate").Find("background").GetComponent<RectTransform>();
            //rt.Translate(pos);
            rt.localPosition = pos;
            //pos.x = rt.sizeDelta.x;
            pos.y -= bgsize.rect.height + 35;
        }

    }

}
