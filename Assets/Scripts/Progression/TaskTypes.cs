using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    protected string name;
    protected bool completed = false;

    public string getName()
    {
        return name;
    }
    abstract public void checkCompletion();
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
        return GameManager.Instance.GetSpendablePaper()[paperType] / amount;
    }
}
