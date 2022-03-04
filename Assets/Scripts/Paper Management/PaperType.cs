using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Paper/Type", order = 1)]
public class PaperType : ScriptableObject
{
    public string PaperName { get { return paperName; } private set { paperName = value; } }
    private string paperName;
    public Texture paperTexture;
    // Have the biomes store the weights?

}
