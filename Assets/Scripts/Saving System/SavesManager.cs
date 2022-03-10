using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public static class SavesManager
{
    private static string extension = ".sav";
    //private static string metaExtension = ".atem";
    private static string savePath = "/Saves/"; // relative to streaming assets path
    public static SaveData StoredData { get { return storedData; } }
    private static SaveData storedData = new SaveData();

    public static void SaveGame(string name)
    {
        string json = JsonConvert.SerializeObject(storedData, Formatting.Indented,
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
#if UNITY_EDITOR
        var sw = new System.IO.StreamWriter(Application.streamingAssetsPath + savePath + name + extension);
#else
        var sw = new System.IO.StreamWriter(Application.persistentDataPath + savePath + name + fileExtension);
#endif
        sw.Write(json);
        sw.Close();
    }

    public static void LoadGame(string name)
    {
#if UNITY_EDITOR
        var sr = new System.IO.StreamReader(Application.streamingAssetsPath + savePath + name + extension);
#else
        var sr = new System.IO.StreamReader(Application.persistentDataPath + savePath + name + extension);
#endif
        storedData = JsonConvert.DeserializeObject<SaveData>(sr.ReadToEnd(),
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        sr.Close();
    }
}

public class SaveData
{
    // World sim will need a converter -- Newtonsoft will take care of the rest
    public Dictionary<string, WorldSim> Terrariums { get { return terrariums; } private set { terrariums = value; } }
    private Dictionary<string, WorldSim> terrariums;
    // Save just the name of the paper type
    public Dictionary<string, int> SpendablePaper { get { return spendablePaper; } private set { spendablePaper = value; } }
    private Dictionary<string, int> spendablePaper;

    public SaveData()
    {
        terrariums = new Dictionary<string, WorldSim>();
        spendablePaper = new Dictionary<string, int>();
    }
}