using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRenderTexture : MonoBehaviour
{
    [SerializeField] RenderTexture t;
    [SerializeField] SaveMinimapTexture destination;
    public void Save()
    {
        destination.t = new Texture2D(256, 256);
        Rect read = new Rect(0, 0, 256, 256);
        RenderTexture.active = t;
        destination.t.ReadPixels(read, 0, 0);
        destination.t.Apply();
        RenderTexture.active = null;
    }
}
