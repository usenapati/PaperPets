using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockContainerScript : MonoBehaviour
{

    [SerializeField] RectTransform bg;

    private void Start()
    {
        bg.sizeDelta = new Vector2(bg.rect.width, (transform.childCount - 1) * 100);
    }

    // Update is called once per frame
    void Update()
    {

        

    }

}
