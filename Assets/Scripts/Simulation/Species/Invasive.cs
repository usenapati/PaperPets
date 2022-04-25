using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Invasive
{

    protected bool unlocked = false;
    protected string species;

    public abstract bool CheckProgress();

    public string GetSpecies()
    {
        return species;
    }

}

public class AphidInvasive : Invasive
{

    float unlockPercent = .01f;

    public AphidInvasive()
    {
        species = "Aphid";
        unlocked = false;
    }

    public override bool CheckProgress()
    {
        if (unlocked) return true;

        if (GameManager.Instance.getCurrentWorld().hasSpecies("Oak"))
        {
            if (Random.Range(0, 1) < unlockPercent)
            {
                unlocked = true;
                Debug.Log("Adding Aphid Invasion!");
            }
        }

        return unlocked;
    }

}