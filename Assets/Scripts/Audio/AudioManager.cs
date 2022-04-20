using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioSource baseAudioPlayer;
    [SerializeField] AudioClip baseTrack;
    [SerializeField] AudioClip loopStem1;
    [SerializeField] AudioClip loopStem2;
    [SerializeField] AudioClip loopStem3;
    [SerializeField] float loopTimeSec;
    [SerializeField] float stemInterval;
    float accumulator;

    AudioSource track1;
    AudioSource track2;
    AudioSource track3;
    [SerializeReference] bool stemsCanPlay = true;
    [SerializeField] float[,] stemIntervals = {
        { 18.300f, 36.891f, 55.187f, 0 },
        { 8.969f, 45.511f, 54.953f, 0 },
        { 18.199f, 36.528f, 54.722f, 0 } };
    [SerializeField] bool[] stemsPlaying = new bool[3];

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
        baseAudioPlayer.clip = baseTrack;
        //baseAudioPlayer.time = 60;
        baseAudioPlayer.Play();

        stemIntervals[0, 3] = loopTimeSec;
        stemIntervals[1, 3] = loopTimeSec;
        stemIntervals[2, 3] = loopTimeSec;

        // scene change
        SceneManager.activeSceneChanged += SceneChanged;

        track1 = gameObject.AddComponent<AudioSource>();
        track1.clip = loopStem1;
        track2 = gameObject.AddComponent<AudioSource>();
        track2.clip = loopStem2;
        track3 = gameObject.AddComponent<AudioSource>();
        track3.clip = loopStem3;
        StartCoroutine(ManageClip(track1, 0));
        StartCoroutine(ManageClip(track2, 1));
        StartCoroutine(ManageClip(track3, 2));
        StartCoroutine(ManagePlayingStems());
    }

    IEnumerator ManageClip(AudioSource clip, int track)
    {

        for (int i = 0; true; i = (i + 1) % 4)
        {
            //i = Mathf.Clamp(i, 0, loopTimeSec);
            if (i >= loopTimeSec) i = 0;
            if (stemsPlaying[track])
            {
                if (!clip.isPlaying)
                {
                    //clip.Stop();
                    clip.time = baseAudioPlayer.time;
                    clip.Play();
                }
                /*if (i == 0)
                {
                    clip.time = 0; yield return new WaitForSeconds(stemIntervals[track, i] - .1f);
                }*/
                yield return new WaitForSeconds(stemIntervals[track, i] - baseAudioPlayer.time - .1f);
            }
            else
            {
                clip.Stop();
                yield return new WaitForSeconds(stemIntervals[track, i] - baseAudioPlayer.time - .1f);
            }
        }
        
    }

    IEnumerator ManagePlayingStems()
    {
        for (; ; )
        {
            if (!stemsCanPlay)
            {
                yield return new WaitForSeconds(1);
                continue;
            }

            if (GameManager.Instance.getCurrentWorld().GetAllSpeciesWithTag("bird").Count > 0) stemsPlaying[1] = true;

            int pop = 0;
            foreach (Species s in GameManager.Instance.getCurrentWorld().GetAllSpeciesWithTag("animal"))
            {
                pop += s.population;
            }
            if (pop > 50) stemsPlaying[2] = true;
            else stemsPlaying[2] = false;

            yield return new WaitForSeconds(1);
        }
    }

    void SceneChanged(Scene current, Scene next)
    {
        if (next.name != "TestScene")
        {
            stemsPlaying[0] = false;
            stemsPlaying[1] = false;
            stemsPlaying[2] = false;
            stemsCanPlay = false;
        }
        else
        {
            stemsCanPlay = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        accumulator += Time.deltaTime;
        if (accumulator >= loopTimeSec)
        {
            baseAudioPlayer.time = 0;
            track1.time = 0;
            track2.time = 0;
            track3.time = 0;
            accumulator = 0;
        }
    }
}
