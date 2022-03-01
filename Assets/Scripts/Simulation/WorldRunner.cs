using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRunner : MonoBehaviour
{

    public WorldSim world;

    // Awake is called before the first frame update
    void Awake()
    {
        world = new WorldSim();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            // print outgoing food
            Debug.Log(world.getOutgoingFoods());
        }
        else if (Input.GetKeyDown("h"))
        {
            // print outgoing habitat
            Debug.Log(world.getOutgoingHabitats());
        }
    }
}
