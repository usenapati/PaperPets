using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;

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
            string path = Application.streamingAssetsPath + savePath + fileName + extension;
            var sw = new System.IO.StreamWriter(path);
#else
            string path = Application.persistentDataPath + savePath + fileName + extension;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var sw = new System.IO.StreamWriter(path);
#endif
            sw.Write(json);
            sw.Close();
        }

        public static SaveData LoadGame(string fileName)
        {
#if UNITY_EDITOR
            string path = Application.streamingAssetsPath + savePath + fileName + extension;
            var sr = new System.IO.StreamReader(path);
#else
            string path = Application.persistentDataPath + savePath + fileName + extension;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var sr = new System.IO.StreamReader(path);
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