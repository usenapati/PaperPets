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
    [SerializeField] float maxScale;
    [SerializeField] float startingScale;
    [SerializeField] float positionChange;

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

    private void drawClique(string mainSpecies, HashSet<string> closed, HashSet<string> remaining, float posX, float posY, int depth)
    {
        Species sp = organisms[mainSpecies];
        float finalScale = .5f * Mathf.Clamp(startingScale * sp.population / totalPop, minScale, maxScale);
        GameObject s = Instantiate(SpeciesCircle, new Vector3(posX, -1, posY), new Quaternion(), transform);

        s.transform.localScale = new Vector3(finalScale, finalScale, 1);
        //s.transform.position += new Vector3(dim.x / -2, 0, dim.z / -2);
        s.transform.position += new Vector3(0, 10, 0);

        s.transform.Find("Species").GetComponent<TextMeshPro>().SetText(sp.name + "\n" + sp.population);
        SpeciesVisualData sd = Resources.Load("Visuals/" + sp.name.Replace(" ", "")) as SpeciesVisualData;
        s.GetComponent<SpriteRenderer>().color = sd == null ? Color.black : sd.speciesColor;
        circles.Add(sp.name, s);
        s.GetComponent<SystemViewCircleData>().setSpecies(sp);

        //closed.Add(mainSpecies);
        if (closed.Count == organisms.Count) return;

        // find 4 new smaller species to be around
        PriorityQueue subspecies = new PriorityQueue();
        HashSet<Species> children = new HashSet<Species>();
        children.UnionWith(sp.outgoingFood);
        children.UnionWith(sp.outgoingHabitat);
        remaining.ExceptWith(closed);
        foreach (string child in remaining)
        {
            if (closed.Contains(organisms[child].name)) continue;
            subspecies.enqueue(organisms[child], sp.getTotalOutgoing().Contains(organisms[child]));
        }

        List<Species> tempList = new List<Species>();
        for (int i = 0; i < 4 && subspecies.Count() > 0; i++)
        {
            Species temp = subspecies.dequeue();
            closed.Add(temp.name);
            tempList.Add(temp);
        }

        float posChangeX = -positionChange / depth;
        float posChangeY = -positionChange / depth;
        foreach (Species child in tempList)
        {
            drawClique(child.name, closed, remaining, posX + posChangeX, posY + posChangeY, depth + 1);
            if (posChangeX * posChangeY > 0) posChangeY = -posChangeY;
            else posChangeX = -posChangeX;
        }

    }

    private void drawCircles()
    {
        Vector3 dim = rect.bounds.size;
        Vector3 circleSize = SpeciesCircle.GetComponent<Renderer>().bounds.size;

        int max = -1;
        Species maxSpecies = null;
        HashSet<string> remaining = new HashSet<string>();

        foreach (Species sp in organisms.Values)
        {
            remaining.Add(sp.name);
            if (sp.getTotalOutgoing().Count > max)
            {
                max = sp.getTotalOutgoing().Count;
                maxSpecies = sp;
            }
        }
        if (maxSpecies == null) return;
        var closed = new HashSet<string>();
        closed.Add(maxSpecies.name);
        drawClique(maxSpecies.name, closed, remaining, 0, 0, 1);
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

    private void updateCircles()
    {
        int totalPop = 0;
        foreach (Species sp in organisms.Values)
        {
            totalPop += sp.population;
        }
        pop.SetText(totalPop.ToString());

        foreach (Species sp in organisms.Values)
        {
            float finalScale = minScale;
            if (totalPop != 0) finalScale = Mathf.Clamp(startingScale * sp.population / totalPop, minScale, maxScale);

            circles[sp.name].transform.localScale = new Vector3(finalScale, finalScale, 1);

            circles[sp.name].transform.Find("Species").GetComponent<TextMeshPro>().SetText(sp.name + "\n" + sp.population);
        }
    }

    private void Update()
    {
        updateCircles();
    }

}

public class PriorityQueue {

    List<Species> s = new List<Species>();
    Dictionary<Species, bool> contained = new Dictionary<Species, bool>();

    public void enqueue(Species sp, bool has)
    {
        s.Add(sp);
        contained[sp] = has;
        bool cont = true;
        for (int i = s.Count - 1; i > 0 && cont; i--)
        {
            if ((contained[s[i - 1]] == true && contained[s[i]] == false) ||
                s[i].getTotalOutgoing().Count > s[i - 1].getTotalOutgoing().Count)
            {
                Species temp = s[i];
                s[i] = s[i - 1];
                s[i - 1] = temp;
            }
            else cont = false;
        }
    }

    public Species dequeue()
    {
        if (s.Count == 0) return null;
        Species sp = s[0];
        s.RemoveAt(0);
        return sp;
    }

    public int Count()
    {
        return s.Count;
    }

}