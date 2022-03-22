using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [Header("Initial Values")]
    // Reference the floor of terrarium
    [SerializeField]
    Vector3 intialPosition;
    [SerializeField]
    int initialLength;
    [SerializeField]
    int initialWidth;
    float lengthScale;
    float widthScale;


    [SerializeField]
    GameObject gridPrefab;
    [SerializeField]
    GameObject basePlane;
    [SerializeField]
    GridList list;

    // Start is called before the first frame update
    void Start()
    {
        list = new GridList(initialLength, 2, initialWidth);
        lengthScale = basePlane.transform.localScale.x / initialLength;
        widthScale = basePlane.transform.localScale.z / initialWidth;
        //lengthScale = gridPrefab.transform.localScale.x;
        //widthScale = gridPrefab.transform.localScale.z;
        // Calculate Initial scale, width, and length from reference
        for (int i = 0; i < initialWidth; i++)
        {
            for (int j = 0; j < initialLength; j++)
            {
                GameObject temp = Instantiate(gridPrefab, gameObject.transform);
                temp.transform.localPosition = new Vector3(intialPosition.x + lengthScale * j, intialPosition.y, intialPosition.z + widthScale * i);
                temp.transform.localScale = new Vector3(lengthScale, temp.transform.localScale.y, widthScale);
                list.addElement(j, 0, i, temp);

                GameObject temp2 = Instantiate(gridPrefab, gameObject.transform);
                temp2.transform.localPosition = new Vector3(intialPosition.x + lengthScale * j, intialPosition.y + temp2.transform.localScale.y, intialPosition.z + widthScale * i);
                temp2.transform.localScale = new Vector3(lengthScale, temp2.transform.localScale.y, widthScale);
                list.addElement(j, 1, i, temp2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    private class GridList
    {
        [SerializeField]
        GameObject[,,] elements;

        public GridList(int x, int y, int z)
        {
            elements = new GameObject[x, y, z];
        }

        public void addElement(int x, int y, int z, GameObject gameObject)
        {
            elements[x, y, z] = gameObject;
        }

        public GameObject getElement(int x, int y, int z)
        {
            return elements[x, y, z];
        }
    }
}
