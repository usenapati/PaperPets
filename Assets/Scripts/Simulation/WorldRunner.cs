using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRunner : MonoBehaviour
{

    public WorldSim world;
    public float updateSpeed;
    float deltaTime;

    // Awake is called before the first frame update
    void Awake()
    {
        world = new WorldSim("new world");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown("f"))
        {
            // print outgoing food
            Debug.Log(world.getOutgoingFoods());
        }
        else if (Input.GetKeyDown("h"))
        {
            // print outgoing habitat
            Debug.Log(world.getOutgoingHabitats());
        }
        else if (Input.GetKeyDown("p"))
        {
            // print outgoing habitat
            Debug.Log(world.getPopulations());
        }*/

        // update world
        deltaTime += Time.deltaTime;
        if (deltaTime >= updateSpeed)
        {
            deltaTime = 0;
            world.updateWorld();
        }
    }

    private void OnApplicationQuit()
    {
        world.closeFiles();
    }

}
