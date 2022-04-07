using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadGameChangeScene(string filename)
    {
        GameManager.Instance.LoadGame(filename);
    }
}
