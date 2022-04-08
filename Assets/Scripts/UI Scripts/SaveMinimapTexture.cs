using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMinimapTexture : MonoBehaviour
{
    public Texture2D t;
    [SerializeField] RenderTexture rt;
    static SaveMinimapTexture _instance;
    public static SaveMinimapTexture Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void Update()
    {
        t = new Texture2D(256, 256);
        Rect read = new Rect(0, 0, 256, 256);
        RenderTexture.active = rt;
        t.ReadPixels(read, 0, 0);
        t.Apply();
        RenderTexture.active = null;
    }

}
