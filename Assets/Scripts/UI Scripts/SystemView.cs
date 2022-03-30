using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemView : MonoBehaviour
{    
    private int totalPop;
    [SerializeField] TextMeshProUGUI pop;
    [SerializeField] Renderer rect;
    [SerializeField] GameObject SpeciesCircle;
    [SerializeField] GameObject FoodLinePrefab;
    [SerializeField] GameObject HabitatLinePrefab;
    [SerializeField] float minScale;
    [SerializeField] float startingScale;

    Dictionary<string, Species> organisms;
    Dictionary<string, GameObject> circles;

    private void Start()
    {
        organisms = new Dictionary<string, Species>();
        circles = new Dictionary<string, GameObject>();

        totalPop = 0;
        foreach (Species s in GameManager.Instance.getCurrentWorld().getAllSpecies())
        {
            totalPop += s.population;
            organisms.Add(s.name, s);
        }

        drawCircles();
        drawLines();

        pop.SetText(totalPop.ToString());
        print("test");
    }

    private void drawCircles()
    {
        Vector3 dim = rect.bounds.size;
        Vector3 circleSize = SpeciesCircle.GetComponent<Renderer>().bounds.size;

        foreach (Species sp in organisms.Values)
        {
            float finalScale = Mathf.Max(minScale, startingScale * sp.population / totalPop);
            GameObject s = Instantiate(SpeciesCircle, new Vector3(Random.Range(circleSize.x, dim.x - circleSize.x), -1, Random.Range(circleSize.z, dim.z - circleSize.z)), new Quaternion(), transform);

            s.transform.localScale = new Vector3(finalScale, finalScale, 1);
            s.transform.position += new Vector3(dim.x / -2, 0, dim.z / -2);
            s.transform.position += new Vector3(0, 10, 0);

            s.transform.Find("Species").GetComponent<TextMeshPro>().SetText(sp.name + "\n" + sp.population);
            SpeciesVisualData sd = Resources.Load("Visuals/" + sp.name.Replace(" ", "")) as SpeciesVisualData;
            s.GetComponent<SpriteRenderer>().color = sd == null ? Color.black : sd.speciesColor;
            circles.Add(sp.name, s);
            s.GetComponent<SystemViewCircleData>().setSpecies(sp);
        }
    }

    // modified from https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
    void DrawQuadraticBezierCurve(LineRenderer lr, Vector3 start, Vector3 mid, Vector3 end)
    {
        lr.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lr.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * start + 2 * (1 - t) * t * mid + t * t * end;
            lr.SetPosition(i, B);
            t += (1 / (float) lr.positionCount);
        }
    }

    private void drawLines()
    {
        foreach (Species s in organisms.Values)
        {
            foreach (Species food in s.outgoingFood)
            {
                GameObject line = Instantiate(FoodLinePrefab, transform);
                LineRenderer lr = line.GetComponent<LineRenderer>();

                Vector3 mid = new Vector3(circles[food.name].transform.position.x, .25f, circles[s.name].transform.position.z);
                DrawQuadraticBezierCurve(lr, circles[s.name].transform.position, mid, circles[food.name].transform.position);
            }

            foreach (Species habitat in s.outgoingHabitat)
            {
                GameObject line = Instantiate(HabitatLinePrefab, transform);
                LineRenderer lr = line.GetComponent<LineRenderer>();

                Vector3 mid = new Vector3(circles[s.name].transform.position.x, .25f, circles[habitat.name].transform.position.z);
                DrawQuadraticBezierCurve(lr, circles[s.name].transform.position, mid, circles[habitat.name].transform.position);
            }
        }
    }

}
