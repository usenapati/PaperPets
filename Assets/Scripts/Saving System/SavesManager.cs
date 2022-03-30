using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace SaveSystem {
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

            sr.Close();
            return saveData;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SaveData
    {
        [JsonProperty]
        private Dictionary<string, WorldSim> terrariums;
        [JsonProperty]
        private Dictionary<string, int> spendablePaper;
        [JsonProperty]
        private ProgressionSystem progressionSystem;

        public SaveData() {}

        public SaveData(Dictionary<string, WorldSim> terrariums, Dictionary<PaperType, int> spendablePaper, ProgressionSystem progressionSystem)
        {
            this.terrariums = terrariums;
            
            this.spendablePaper = new Dictionary<string, int>();
            foreach (KeyValuePair<PaperType, int> kv in spendablePaper)
            {
                this.spendablePaper[kv.Key.PaperName] = kv.Value;
            }

            this.progressionSystem = progressionSystem;
        }

        public Dictionary<PaperType, int> GetSpendablePaper()
        {
            Dictionary<PaperType, int> reloadedSpendable = new Dictionary<PaperType, int>();
            foreach (KeyValuePair<string, int> kv in spendablePaper)
            {
                reloadedSpendable[Resources.Load("Paper/" + kv.Key) as PaperType] = kv.Value;
            }
            return reloadedSpendable;
        }

        public Dictionary<string, WorldSim> GetTerrariums()
        {
            foreach (WorldSim w in terrariums.Values)
            {
                w.onLoadIn();
            }
            return terrariums;
        }

        public ProgressionSystem GetProgressionSystem()
        {
            progressionSystem.setup();
            return progressionSystem;
        }

    }
}