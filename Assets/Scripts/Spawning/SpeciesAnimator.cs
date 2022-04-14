using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float animSwitch;
    [SerializeField] float randomRange;
    [SerializeField] GameObject debug;

    public SpeciesVisualData sv;

    // sprite animation
    bool frame = false;
    float elapsed = 0;
    float maxTime = 0;

    // path animation
    Vector3 startPoint;
    List<Vector3> path;
    int pathIndex;
    float pathElapsed;
    float pathTimeSeconds;
    bool regeneratePath = false;
    bool debugOn = false;

    private void Start()
    {
        startPoint = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(animController());
        StartCoroutine(pathController());
    }

    IEnumerator animController()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= maxTime)
        {
            elapsed = 0;
            maxTime = sv.animationTime + Random.Range(-randomRange * sv.animationTime, randomRange * sv.animationTime);
            frame = !frame;
            sr.sprite = frame ? sv.sprite2 : sv.sprite;
        }
        yield return new WaitForSeconds(maxTime);
    }

    Vector3 RandomVector()
    {
        return new Vector3(Random.Range(-sv.pathRandomnessX, sv.pathRandomnessX), Random.Range(-sv.pathRandomnessY, sv.pathRandomnessY), Random.Range(-sv.pathRandomnessZ, sv.pathRandomnessZ));
    }

    void GenerateCircularPath()
    {
        path = new List<Vector3>();
        Vector3 scale = new Vector3(sv.radius, sv.radius, sv.radius);
        path.Add(RandomVector() + startPoint + Vector3.Scale(Vector3.left, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale((Vector3.left + Vector3.forward).normalized, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale(Vector3.forward, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale((Vector3.forward + Vector3.right).normalized, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale(Vector3.right, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale((Vector3.right + Vector3.back).normalized, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale(Vector3.back, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale((Vector3.back + Vector3.left).normalized, scale));
    }

    void GenerateErraticPath()
    {

        if (path == null)
        {
            GenerateCircularPath();
            return;
        }

        Vector3 initPoint = path[0];
        Vector3 scale = new Vector3(sv.radius, sv.radius, sv.radius);
        path.Add(initPoint);
        path.Add(RandomVector() + startPoint + Vector3.Scale((Vector3.left + Vector3.forward).normalized, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale(Vector3.forward, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale((Vector3.forward + Vector3.right).normalized, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale(Vector3.right, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale((Vector3.right + Vector3.back).normalized, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale(Vector3.back, scale));
        path.Add(RandomVector() + startPoint + Vector3.Scale((Vector3.back + Vector3.left).normalized, scale));

    }

    void GeneratePath()
    {
        pathTimeSeconds = sv.pathTimeSeconds * Random.Range(.9f, 1.1f);
        switch (sv.pathType)
        {
            case SpeciesPathType.NONE:
                path = new List<Vector3>();
                return;
            case SpeciesPathType.CIRCULAR:
                GenerateCircularPath();
                break;
            case SpeciesPathType.ERRATIC:
                GenerateErraticPath();
                regeneratePath = true;
                break;
            default:
                break;
        }
    }

    // from example here: https://www.gamedeveloper.com/programming/path-animation-in-unity
    public static Vector3 CatmullInterpolation(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return ((p1 * 2.0f)
            + (-p0 + p2) * t
            + ((p0 * 2.0f) - (p1 * 5.0f) + (p2 * 4.0f) - p3) * (t * t)
            + (-1 * p0 + (p1 * 3.0f) - (p2 * 3.0f) + p3) * (t * t * t)) * 0.5f;
    }

    IEnumerator pathController()
    {
        if (path == null) GeneratePath();

        pathElapsed += Time.deltaTime;
        if (path.Count != 0)
        {
            gameObject.transform.position = CatmullInterpolation(path[pathIndex], path[(pathIndex + 1) % path.Count], path[(pathIndex + 2) % path.Count], path[(pathIndex + 3) % path.Count], pathElapsed / pathTimeSeconds);
            if (debugOn) Instantiate(debug, transform.position, new Quaternion());
        }
        if (path.Count != 0 && pathElapsed >= pathTimeSeconds)
        {
            pathElapsed = 0;
            pathIndex++;
            pathIndex %= path.Count;
            if (regeneratePath) GeneratePath();
            if (pathIndex == 0) debugOn = false;
        }

        yield return null;
    }

}
