using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public static class SavesManager
{
    private static string extension = ".sav";
    private static string savePath = "/Saves/"; // relative to streaming assets path

    public static void SaveGame(string fileName, SaveData saveData)
    {
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented,
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
#if UNITY_EDITOR
        var sw = new System.IO.StreamWriter(Application.streamingAssetsPath + savePath + fileName + extension);
#else
        var sw = new System.IO.StreamWriter(Application.persistentDataPath + savePath + name + fileExtension);
#endif
        sw.Write(json);
        sw.Close();
    }

    public static SaveData LoadGame(string fileName)
    {
#if UNITY_EDITOR
        var sr = new System.IO.StreamReader(Application.streamingAssetsPath + savePath + fileName + extension);
#else
        var sr = new System.IO.StreamReader(Application.persistentDataPath + savePath + name + extension);
#endif
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(sr.ReadToEnd(),
            new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        foreach (WorldSim w in saveData.Terrariums.Values)
        {
            w.onLoadIn();
        }
        sr.Close();
        return saveData;
    }
}

public class SaveData
{
    public Dictionary<string, WorldSim> Terrariums { get { return terrariums; } private set { terrariums = value; } }
    private Dictionary<string, WorldSim> terrariums;
    public Dictionary<string, int> SpendablePaper { get { return spendablePaper; } private set { spendablePaper = value; } }
    private Dictionary<string, int> spendablePaper;

    public SaveData(Dictionary<string, WorldSim> terrariums, Dictionary<PaperType, int> spendablePaper)
    {
        this.terrariums = terrariums;
        this.spendablePaper = new Dictionary<string, int>();
        foreach (KeyValuePair<PaperType, int> kv in spendablePaper)
        {
            this.spendablePaper[kv.Key.name] = kv.Value;
        }
    }
}