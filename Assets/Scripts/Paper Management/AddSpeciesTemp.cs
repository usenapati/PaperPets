using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeciesTemp : MonoBehaviour
{

    public void addSpecies(SpeciesType t)
    {
        GameManager.Instance.addSpecies(t);
    }

}
