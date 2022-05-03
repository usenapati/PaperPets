using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadMinimap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] RawImage r;
    [SerializeField] SceneController sceneManager;
    void Start()
    {
        if (SaveMinimapTexture.Instance != null) r.texture = SaveMinimapTexture.Instance.t;
    }

    public void OnPointerClick(PointerEventData e)
    {
        sceneManager.ChangeScene("SystemView");
    }
}
