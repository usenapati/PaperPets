using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.Fields)]
public class Biome
{

    float temp;
    float humidity;
    float windSpeed;

    [JsonIgnore] Vector2 windDirection;
    [JsonIgnore] string soiltype;

    public Biome(float temp, float humidity, float windSpeed)
    {
        this.temp = temp;
        this.humidity = humidity;
        this.windSpeed = windSpeed;
    }

    public float getTemp()
    {
        return temp;
    }

    public float getHumidity()
    {
        return humidity;
    }

    public float getWindSpeed()
    {
        return windSpeed;
    }

    public void updateBiome((float, float, float, float) newData)
    {
        temp = newData.Item1 / newData.Item4;
        humidity = newData.Item2 / newData.Item4;
        windSpeed = newData.Item3 / newData.Item4;
    }

    public override string ToString()
    {
        return "Biome Info:\n" + "Temperature: " + temp + "\nHumidity: " + humidity + "\nWind Speed: " + windSpeed;
    }

}
