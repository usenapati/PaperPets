using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    [SerializeField] ParticleSystem weather;
    [SerializeField] ParticleSystemRenderer weatherRenderer;
    [SerializeField] float temp;
    [SerializeField] float humidity;
    [SerializeField] float windSpeed;
    [SerializeField] Vector2 windDirection;

    [SerializeField] float maxWeatherDuration;
    [SerializeField] Material rain;
    [SerializeField] Material snow;
    [SerializeField] Gradient snowGradient;
    [SerializeField] Gradient rainGradient;

    [SerializeField] float timeToWait;
    float accumulator = 0;
    bool raining = false;
    Gradient g;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        accumulator += Time.deltaTime;
        if (accumulator > timeToWait)
        {
            if (raining)
            {
                raining = false;
                timeToWait = (1 - humidity) * maxWeatherDuration;
                weather.Stop();
            }
            else
            {

                var main = weather.main;

                raining = true;
                timeToWait = humidity * maxWeatherDuration;

                if (temp < 32)
                {
                    weatherRenderer.material = snow;
                    main.gravityModifier = .5f;
                    var noise = weather.noise;
                    noise.enabled = true;
                    noise.frequency = .1f;
                    var color = weather.colorOverLifetime;
                    color.color = snowGradient;
                }
                else
                {
                    weatherRenderer.material = rain;
                    main.gravityModifier = 1;
                    var noise = weather.noise;
                    noise.enabled = false;
                    var color = weather.colorOverLifetime;
                    color.color = rainGradient;
                }

                main.duration = timeToWait;
                weather.Play();

            }
            accumulator = 0;
        }

    }
}
