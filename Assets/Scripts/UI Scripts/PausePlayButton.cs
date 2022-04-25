using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePlayButton : MonoBehaviour
{
    [SerializeField] Sprite pauseGraphic;
    [SerializeField] Sprite playGraphic;
    [SerializeField] Image img;

    bool paused = false;

    public void toggle()
    {
        GameManager.Instance.togglePause();
        paused = !paused;
        img.sprite = paused ? playGraphic : pauseGraphic;
    }
}
