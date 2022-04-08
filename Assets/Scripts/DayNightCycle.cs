using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class DayNightCycle : MonoBehaviour
{
    public static string tickEventName = "daycycle";
    public Transform sun;
    public AnimationCurve sunBrightness = new AnimationCurve(
        new Keyframe(0, 0.01f),
        new Keyframe(0.15f, 0.01f),
        new Keyframe(0.35f, 1),
        new Keyframe(0.65f, 1),
        new Keyframe(0.85f, 0.01f),
        new Keyframe(1, 0.01f)
    );

    public Gradient sunColor = new Gradient()
    {
        colorKeys = new GradientColorKey[2]{
        new GradientColorKey(new Color(1, 0.75f, 0.3f), 0.45f),
        new GradientColorKey(new Color(0.95f, 0.95f, 1), 0.75f),
        },
        alphaKeys = new GradientAlphaKey[2]{
        new GradientAlphaKey(1, 0),
        new GradientAlphaKey(1, 1)
        }
    };
    [GradientUsage(true)]
    public Gradient skyColorDay = new Gradient()
    {
        colorKeys = new GradientColorKey[3]{
            new GradientColorKey(new Color(0.75f, 0.3f, 0.17f), 0),
            new GradientColorKey(new Color(0.7f, 1.4f, 3), 0.5f),
            new GradientColorKey(new Color(0.75f, 0.3f, 0.17f), 1),
        },
        alphaKeys = new GradientAlphaKey[2]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        }
    };
    [GradientUsage(true)]
    public Gradient skyColorNight = new Gradient()
    {
        colorKeys = new GradientColorKey[3]{
            new GradientColorKey(new Color(0.75f, 0.3f, 0.17f), 0),
            new GradientColorKey(new Color(0.44f, 1, 1), 0.5f),
            new GradientColorKey(new Color(0.75f, 0.3f, 0.17f), 1),
        },
        alphaKeys = new GradientAlphaKey[2]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        }
    };
    public float starsSpeed = 8;
    public Vector2 cloudsSpeed = new Vector2(0.1f, 0.1f);
    public float sunTimeSpeed = 0.1f;
    [Min(2f)]
    public float cyclesPerTick = 20f;

    public Material customSkyMaterialCheck;

    private Light sunLight;
    private float sunAngle;
    private float dt = 1f;

    // Start is called before the first frame update
    void Start()
    {
        sun.rotation = Quaternion.Euler(0, -90, 0);
        sunLight = sun.GetComponent<Light>();
        RenderSettings.skybox.SetVector("_CloudOffset", new Vector2(0,0));
        if (GameManager.Instance.getUnityTickEvent(tickEventName) == null)
        {
            UnityEvent<float> tickEvent = new UnityEvent<float>();
            StartCoroutine(TicksCoroutine());
            tickEvent.AddListener(SetSkyTickdt);
            GameManager.Instance.addUnityEvent(tickEventName, tickEvent);
        }
    }

    // Update skybox a single tick
    private void Tick()
    {
        if (!(RenderSettings.skybox.ToString() == customSkyMaterialCheck.ToString()))
            return;
        if (Application.isPlaying)
        {
            RotateSun();
            MoveClouds();
        }
        sunAngle = (Vector3.SignedAngle(Vector3.down, sun.forward, sun.right) / 360) + 0.5f;
        SetSunBrightness();
        SetSunColor();
        SetSkyColor();
        MoveStars();
    }

    void SetSunBrightness()
    {
        sunLight.intensity = sunBrightness.Evaluate(sunAngle);
    }
    void SetSunColor()
    {
        sunLight.color = sunColor.Evaluate(sunAngle);
    }
    void SetSkyColor()
    {
        if (sunAngle >= 0.25f && sunAngle < 0.75f)
            RenderSettings.skybox.SetColor("_SkyColor2", skyColorDay.Evaluate(sunAngle * 2f - 0.5f));
        else if (sunAngle > 0.75f)
            RenderSettings.skybox.SetColor("_SkyColorNight2", skyColorNight.Evaluate(sunAngle * 2f - 1.5f));
        else
            RenderSettings.skybox.SetColor("_SkyColorNight2", skyColorNight.Evaluate(sunAngle * 2f + 0.5f));
    }
    void MoveStars()
    {
        RenderSettings.skybox.SetVector("_StarsOffset", new Vector2(sunAngle * starsSpeed, 0));
    }
    void MoveClouds()
    {
        RenderSettings.skybox.SetVector("_CloudOffset", (Vector2)RenderSettings.skybox.GetVector("_CloudOffset") + cloudsSpeed);
    }
    void RotateSun()
    {
        sun.Rotate(Vector3.right * sunTimeSpeed);
    }

    void SetSkyTickdt(float dt)
    {
        this.dt = dt;
 //       StartCoroutine(TicksCoroutine());
    }
    IEnumerator TicksCoroutine()
    {
        while(true)
        {
            Tick();
            yield return new WaitForSeconds(dt / cyclesPerTick);
        }
    }
}
