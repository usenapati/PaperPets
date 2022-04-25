using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvasivesManager
{

    float accumulator = 0;
    bool check = false;

    private void AddInvasive(string s)
    {
        if (GameManager.Instance.getCurrentWorld().hasSpecies(s)) return;

        SpeciesType st = Resources.Load("Species/" + s) as SpeciesType;
        if (st == null) return;

        GameManager.Instance.addSpecies(st);
    }

    public void Update()
    {
        if (check) return;

        //accumulator += Time.deltaTime;
        if (GameManager.Instance.getCurrentWorld().hasSpecies("Monarch Butterfly"))
        {
            check = true;
            AddInvasive("Aphid");
        }

    }

}
