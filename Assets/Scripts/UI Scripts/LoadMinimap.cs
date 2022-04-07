using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMinimap : MonoBehaviour
{
    [SerializeField] RawImage r;
    void Start()
    {
        if (SaveMinimapTexture.Instance != null) r.texture = SaveMinimapTexture.Instance.t;
    }
}
