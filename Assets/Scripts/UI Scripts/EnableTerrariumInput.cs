using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTerrariumInput : MonoBehaviour
{
    [SerializeField] GameObject terrariumInput;

    public void EnableTerrariumCreation()
    {
        terrariumInput.SetActive(true);
    }
}
