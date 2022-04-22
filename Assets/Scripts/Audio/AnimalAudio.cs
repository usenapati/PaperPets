using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAudio : MonoBehaviour
{
    private AudioSource source;

    private AudioClip clip;

    public SpeciesVisualData speciesVisualData;

    private float minSpawnTime;
    private float maxSpawnTime;
    
    void Start()
    {
        speciesVisualData = GetComponent<SpeciesAnimator>().sv;
        minSpawnTime = speciesVisualData.minSpawnAudioTime;
        maxSpawnTime = speciesVisualData.maxSpawnAudioTime;
        source = GetComponent<AudioSource>();
        source.volume = speciesVisualData.volume;
        if (minSpawnTime != 0 || maxSpawnTime != 0)
        {
            Invoke("PlayRandomSound", (Random.Range(minSpawnTime, maxSpawnTime)));
        }
    }
    
    void PlayRandomSound()
    {
        float delay = Random.Range (minSpawnTime, maxSpawnTime);
        int index = Random.Range(0, speciesVisualData.sounds.Length);
        clip = speciesVisualData.sounds[index];
        source.clip = clip;
        source.Play();
        Debug.Log("Played Sound");
        Invoke ("PlayRandomSound", delay);
    }
}
