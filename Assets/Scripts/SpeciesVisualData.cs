using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Species Visuals", menuName = "Species/New Visual")]
public class SpeciesVisualData : ScriptableObject
{
    public string speciesName;
    
    public Sprite sprite;

    public Vector3 scale;

    public int layerHeight;

    public float height;

    public int speciesLimit;

    public int conversionValue;

    public Color speciesColor;
}
