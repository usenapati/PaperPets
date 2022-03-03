using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Paper/Type", order = 1)]
public class PaperType : ScriptableObject
{
    public string paperName = "";
    public Texture paperTexture;
    // Have the biomes store the weights?
    public string biomeWeights;
}
