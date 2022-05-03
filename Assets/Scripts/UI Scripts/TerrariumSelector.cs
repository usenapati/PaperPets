using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TerrariumSelector : MonoBehaviour
{

    GameObject terrariumButton;
    [SerializeField] TextMeshProUGUI terrariumNameField;

    string terrariumID;
    string terrariumName;

    public void SetTerrariumData(string terrariumID, string terrariumName)
    {
        this.terrariumID = terrariumID;
        this.terrariumName = terrariumName;
        terrariumNameField.text = terrariumName;
    }

    public void OpenTerrarium()
    {
        GameManager.Instance.SwitchCurrentWorld(terrariumID);
        SceneManager.LoadScene("TestScene");
    }
}
