using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSave : MonoBehaviour
{
    public string saveName = "Save";
    public WorldSim worldSim;
    public SpeciesType firstSpecies;
    public SpeciesType secondSpecies;
    public SpeciesType thirdSpecies;
    WorldSim temp1 = new WorldSim("hue");
    WorldSim temp2 = new WorldSim("tehorld");

    WorldSim temp3;
    WorldSim temp4;

    Dictionary<string, WorldSim> terrariums = new Dictionary<string, WorldSim>();
    Dictionary<PaperType, int> papermoney = new Dictionary<PaperType, int>();

    ProgressionSystem p;

    public void Save()
    {


        temp1.addSpecies(firstSpecies);
        temp1.addSpecies(secondSpecies);
        temp1.addSpecies(thirdSpecies);
        temp2.addSpecies(secondSpecies);

        //temp1.addSpecies(species1);
        //temp1.addSpecies(species2);
        //temp2.addSpecies(species2);
        terrariums["testworld"] = temp1;
        terrariums["test2"] = temp2;

        p = new ProgressionSystem();

        SaveSystem.SavesManager.SaveGame(saveName, new SaveSystem.SaveData(terrariums, papermoney, p, new Dictionary<string, bool>()));
    }

    public void Load()
    {
        //
        SaveSystem.SavesManager.LoadGame(saveName);
        Dictionary<string, WorldSim> temp = SaveSystem.SavesManager.LoadGame(saveName).GetTerrariums();

        Debug.Log(temp.Count);
        Debug.Log(temp["testworld"].getOutgoingFoods());
        Debug.Log(temp["testworld"].getOutgoingHabitats());
    }
}
