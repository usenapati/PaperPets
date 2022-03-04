using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Pause_Menu : MonoBehaviour
{

    private Transform container;
    private Transform template;
    private bool onScreen = false;

    // Start is called before the first frame update
    private void Awake()
    {
        container = transform.Find("pauseContainer");
        template = container.Find("pauseTemplate");
        template.gameObject.SetActive(false);
    }

    public void pauseMenu()
    {
        Transform menu = Instantiate(template, container);
        RectTransform menuTransform = menu.GetComponent<RectTransform>();
        if(!onScreen)
        {
            Vector3 position = new Vector3(900,600,0);
            menuTransform.position = position;
            template.gameObject.SetActive(true);
            onScreen = true;
        }
        else{
            Vector3 position = new Vector3(5000,5000,0);
            menuTransform.position = position;
            template.gameObject.SetActive(false);
            
            onScreen = false;
        }

         
      
    }

    
}
