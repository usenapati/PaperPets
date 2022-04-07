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

    List<KeyValuePair<string, int>> speciesPopulation;
    Dictionary<string, SpeciesVisualData> organisms = new Dictionary<string, SpeciesVisualData>();
    Dictionary<string, List<GameObject>> organismsInScene = new Dictionary<string, List<GameObject>>();
    // Random Value Generation

    // Start is called before the first frame update
    void Start()
    {
        gridSpawner = FindObjectOfType<GridSpawner>();
        for (int i = 0; i < 100; i++)
        {
            //SpawnOrganism("Milkweed");
        }
        
        
    }

    private void Update()
    {
        //Check WorldSim
        speciesPopulation = GameManager.Instance.getCurrentWorld().getAllSpeciesPopulation();
        //Debug.Log(speciesPopulation.Count);
        //Loop through list
        for (int i = 0; i < speciesPopulation.Count; i++)
        {
            string speciesName = speciesPopulation[i].Key;
            if (!organisms.ContainsKey(speciesName))
            {
                AddSpeciesToDictionary(speciesName);
            }
            Debug.Log("Species name: " + speciesName);
            if (organisms[speciesName] != null)
            {
                int difference = organismsInScene[speciesName].Count - organisms[speciesName].conversionValue * speciesPopulation[i].Value;
                Debug.Log("difference: " + difference);
                // Deleting Organisms
                if (difference > 0)
                {
                    for (int j = 0; j < difference; j++)
                    {
                        GameObject gameObject = organismsInScene[speciesName][organismsInScene[speciesName].Count - 1];
                        organismsInScene[speciesName].Remove(gameObject);
                        Destroy(gameObject);
                    }
                }
                // Adding Organisms
                else if (difference < 0)
                {
                    for (int j = 0; j < -difference; j++)
                    {
                        Debug.Log("Spawning " + speciesName);
                        GameObject gameObject = SpawnOrganism(speciesName);
                        organismsInScene[speciesName].Add(gameObject);
                    }
                }
            }
        }
        //If species num > limit
        //Don't spawn them
        //Else
        //Spawn them based on conversion val
    }

    void AddSpeciesToDictionary(string organismName)
    {
        string trimmed = organismName.Replace(" ", "");
        Debug.Log("Original: " + organismName + " Trimmed: " + trimmed);
        organisms.Add(organismName, Resources.Load("Visuals/" + trimmed) as SpeciesVisualData);
        organismsInScene.Add(organismName, new List<GameObject>());
    }

    public GameObject SpawnOrganism(string speciesName)
    {
        SpeciesVisualData organism = organisms[speciesName];
        
        int layer = organism.layerHeight;
        float height = organism.height;
        
        (float, float) spawnPlanePosition = GenerateRandomPosition(gridElementPrefab.transform.localScale.x, gridElementPrefab.transform.localScale.z);
        Vector3 spawnPosition = new Vector3(spawnPlanePosition.Item1, height, spawnPlanePosition.Item2);
        (int, int, int) gridPosition = GenerateGridPosition(layer);
        GameObject gridElement = gridSpawner.getGridElement(gridPosition.Item1, gridPosition.Item2, gridPosition.Item3);
        GameObject temp = Instantiate(organismPrefab, gridElement.transform);
        temp.transform.localPosition = spawnPosition;
        temp.GetComponent<SpriteRenderer>().sprite = organism.sprite;
        temp.transform.localScale = organism.scale;
        return temp;
    }

    public void SpawnOrganismAtElement(string speciesName, int x, int z)
    {
        SpeciesVisualData organism = organisms[speciesName];

        int layer = organism.layerHeight;
        float height = organism.height;

        (float, float) spawnPlanePosition = GenerateRandomPosition(gridElementPrefab.transform.localScale.x, gridElementPrefab.transform.localScale.z);
        Vector3 spawnPosition = new Vector3(spawnPlanePosition.Item1, height, spawnPlanePosition.Item2);
        GameObject gridElement = gridSpawner.getGridElement(x, layer, z);
        GameObject temp = Instantiate(organismPrefab, gridElement.transform);
        temp.transform.localPosition = spawnPosition;
        temp.GetComponent<SpriteRenderer>().sprite = organism.sprite;
        temp.transform.localScale = organism.scale;
    }

    public void SpawnRandomOrganism()
    {

        int x = Random.Range(0, 3);
        Debug.Log(x);
        switch (x)
        {
            case 0:
                SpawnOrganism("Milkweed");
                break;
            case 1:
                SpawnOrganism("MonarchButterfly");
                break;
            case 2:
                SpawnOrganism("Oriole");
                break;
            case 3:
                SpawnOrganism("Eagle");
                break;
            default:
                break;
        }
    }

    (int, int, int) GenerateGridPosition (int Layer)
    {
        int x = Random.Range(0, gridSpawner.getGridLength());
        int y = Layer;
        int z = Random.Range(0, gridSpawner.getGridWidth());
        //Debug.Log("(" + x + "," + y + "," + z + ")");
        return (x, y, z);
    }

    (float, float) GenerateRandomPosition(float scaleX, float scaleZ)
    {
        float x = Random.Range(-scaleX/2, scaleX/2);
        float z = Random.Range(-scaleZ/2, scaleZ/2);
        //Debug.Log("(" + x + "," + 0 + "," + z + ")");
        return (x, z);
    }
}
