using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightWaterDisplay : MonoBehaviour
{

    [SerializeField] Text textbox;

    // Update is called once per frame
    void Update()
    {

        textbox.text = string.Format("Available Light: {0}\n     Light Lv: {1}\nAvailable Water: {2}\n     Water Lv: {3}",
            GameManager.Instance.getCurrentWorld().availableLight, GameManager.Instance.getCurrentWorld().availableLight / 50,
            GameManager.Instance.getCurrentWorld().availableTotalWater, GameManager.Instance.getCurrentWorld().availableTotalWater / 100);

    }
}
