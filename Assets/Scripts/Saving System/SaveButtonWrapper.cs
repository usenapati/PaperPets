using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveButtonWrapper : MonoBehaviour
{
    public void SaveGame(string filename)
    {
        GameManager.Instance.SaveGame(filename);
        Debug.Log("Exit Button clicked"); 
        #if UNITY_EDITOR 
        // Application.Quit() does not work in the editor so 
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game 
            UnityEditor.EditorApplication.isPlaying = false; 
        #else 
            Application.Quit(); 
        #endif 
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
