using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButtonWrapper : MonoBehaviour
{
    public void SaveGame(string filename)
    {
        GameManager.Instance.SaveGame(filename);
    }

    public void LoadGame(string filename)
    {
        GameManager.Instance.LoadGame(filename);
    }
}
