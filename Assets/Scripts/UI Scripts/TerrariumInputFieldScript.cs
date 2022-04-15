using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TerrariumInputFieldScript : MonoBehaviour
{

    [SerializeField] TMP_InputField input;
    [SerializeField] TerrariumContainerManager tcm;
    [SerializeField] GameObject toDisable;

    bool update = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (update)
        {
            update = false;
            tcm.CreateTerrarium(input.text);
            input.text = "";
            toDisable.SetActive(false);
        }
    }

    public void OnAccept(InputAction.CallbackContext context)
    {
        update = true;
    }

}
