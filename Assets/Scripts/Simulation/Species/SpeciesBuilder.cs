using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesBuilder : MonoBehaviour
{
    public WorldRunner world;

    [SerializeField]
    SpeciesType type;

    public Species s;

    // Start is called before the first frame update
    void Start()
    {
        s = new Species(type, world.world);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
