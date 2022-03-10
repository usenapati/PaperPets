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

    public void Save()
    {


        temp1.addSpecies(firstSpecies);
        temp1.addSpecies(secondSpecies);
        temp1.addSpecies(thirdSpecies);
        temp2.addSpecies(secondSpecies);

        //temp1.addSpecies(species1);
        //temp1.addSpecies(species2);
        //temp2.addSpecies(species2);

        SavesManager.StoredData.Terrariums["testworld"] = temp1;
        SavesManager.StoredData.Terrariums["test2"] = temp2;
        SavesManager.SaveGame(saveName);
    }

    public void Load()
    {
        //
        SavesManager.LoadGame(saveName);
        Dictionary<string, WorldSim> temp = SavesManager.StoredData.Terrariums;

        Debug.Log(temp.Count);
        Debug.Log(temp["testworld"].getOutgoingFoods());
        Debug.Log(temp["testworld"].getOutgoingHabitats());
    }
}
