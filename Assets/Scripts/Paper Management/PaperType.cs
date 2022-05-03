using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Paper/Type", order = 1)]
public class PaperType : ScriptableObject
{
    public string PaperName { get { return paperName; } private set { paperName = value; } }
    [SerializeField]
    private string paperName;
    public Texture paperTexture;
    // Have the biomes store the weights?

}