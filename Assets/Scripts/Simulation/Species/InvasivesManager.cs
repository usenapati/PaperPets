using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvasivesManager
{

    bool check = false;
    HashSet<Invasive> invasivesWaiting;
    HashSet<Invasive> toRemove;

    public InvasivesManager()
    {
        invasivesWaiting = new HashSet<Invasive>();
        invasivesWaiting.Add(new AphidInvasive());
        toRemove = new HashSet<Invasive>();
    }

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

        foreach (Invasive i in invasivesWaiting)
        {
            if (i.CheckProgress())
            {
                toRemove.Add(i);
                AddInvasive(i.GetSpecies());
            }
        }

        invasivesWaiting.ExceptWith(toRemove);
        toRemove.Clear();
    }

}
