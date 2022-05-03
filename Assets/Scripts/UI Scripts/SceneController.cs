using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene(string s){
        SceneManager.LoadScene(s);
    }

    public void LoadScene(string s){
        SceneManager.LoadScene(s);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Button clicked");
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
