using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlicker : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;
    [SerializeField, TextArea] string swapText;
    string temp;
    float sec;

    // Update is called once per frame
    void Update()
    {

        sec += Time.deltaTime;
        if (sec >= 2)
        {
            sec = 0;
            temp = text.text;
            text.text = swapText;
            swapText = temp;
        }

    }
}
