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


}
