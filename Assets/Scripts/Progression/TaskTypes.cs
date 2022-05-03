using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    protected string name;
    protected bool completed = false;
    protected bool visible = true;

    public string getName()
    {
        return name;
    }
    abstract public void checkCompletion();
    public bool isVisible()
    {
        return visible;
    }
    public bool complete()
    {
        return completed;
    }
    abstract public float progress();
}

public class PaperTask : Task
{
    PaperType paperType;
    float amount;

    public PaperTask(string name, PaperType paperType, float amount)
    {
        this.name = name;
        this.paperType = paperType;
        this.amount = amount;
    }

    public override void checkCompletion()
    {
        if (GameManager.Instance.GetSpendablePaper()[paperType] >= amount)
            completed = true;
    }

    public override float progress()
    {
        if (completed) return 1;
        return Mathf.Min(1, GameManager.Instance.GetSpendablePaper()[paperType] / amount);
    }
}

public class SpeciesTask : Task
{

    string speciesName;

    public SpeciesTask(string name, string speciesName)
    {
        this.name = name;
        this.speciesName = speciesName;
        visible = false;
    }

    public override void checkCompletion()
    {
        foreach (Species s in GameManager.Instance.getCurrentWorld().getAllSpecies())
        {
            if (s.name == speciesName)
            {
                completed = true;
                return;
            }
        }
    }

    public override float progress()
    {
        if (completed) return 1;
        return 0;
    }
}
