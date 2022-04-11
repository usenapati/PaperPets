using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Species Visuals", menuName = "Species/New Visual")]
public class SpeciesVisualData : ScriptableObject
{
    public string speciesName;
    
    public Sprite sprite;
    public Sprite sprite2;

    public SpeciesPathType pathType;
    public float pathTimeSeconds;
    public float radius;
    public float pathRandomnessX;
    public float pathRandomnessY;
    public float pathRandomnessZ;

    public Vector3 scale;

    public int layerHeight;

    public float height;

    public int speciesLimit;

    public int conversionValue;

    public Color speciesColor;
}

public enum SpeciesPathType
{
    NONE = 0,
    CIRCULAR = 100,
    ERRATIC = 200
}