using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesSpawning : MonoBehaviour
{
    [Header("Species")]
    [Tooltip("References to Species Prefabs")]
    [SerializeField]
    GameObject organismPrefab;

    [SerializeField]
    GameObject gridElementPrefab;

    [SerializeField]
    GridSpawner gridSpawner;

    // Random Value Generation

    // Start is called before the first frame update
    void Start()
    {
        gridSpawner = FindObjectOfType<GridSpawner>();
        for (int i = 0; i < 1; i++)
        {
            SpawnOrganism("Milkweed");
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnOrganism(string speciesName)
    {
        SpeciesVisualData organism = Resources.Load("Visuals/" + speciesName) as SpeciesVisualData;
        
        int layer = organism.layerHeight;
        float height = organism.height;
        
        (float, float) spawnPlanePosition = GenerateRandomPosition(gridElementPrefab.transform.localScale.x, gridElementPrefab.transform.localScale.z);
        Vector3 spawnPosition = new Vector3(spawnPlanePosition.Item1, height, spawnPlanePosition.Item2);
        (int, int, int) gridPosition = GenerateGridPosition(layer);
        GameObject gridElement = gridSpawner.getGridElement(gridPosition.Item1, gridPosition.Item2, gridPosition.Item3);
        GameObject temp = Instantiate(organismPrefab, spawnPosition, new Quaternion(), gridElement.transform);
        temp.GetComponent<SpriteRenderer>().sprite = organism.sprite;
        temp.transform.localScale = organism.scale;
    }

    (int, int, int) GenerateGridPosition (int Layer)
    {
        int x = Random.Range(0, gridSpawner.getGridLength() - 1);
        int y = Layer;
        int z = Random.Range(0, gridSpawner.getGridWidth() - 1);
        Debug.Log("(" + x + "," + y + "," + z + ")");
        return (x, y, z);
    }

    (float, float) GenerateRandomPosition(float scaleX, float scaleZ)
    {
        // FIX ME
        // Not spawning within range
        float x = Random.Range(-scaleX/2, scaleX/2);
        float z = Random.Range(-scaleZ/2, scaleZ/2);
        Debug.Log("(" + x + "," + 0 + "," + z + ")");
        return (x, z);
    }
}
