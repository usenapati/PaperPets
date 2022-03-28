using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSystem
{

    // next id of a newly created task
    int nextID;

    // unlocked items
    HashSet<string> unlockedSpecies;
    HashSet<string> otherUnlocks;

    // in progress items
    HashSet<Unlock> inProgress;

    public ProgressionSystem()
    {
        nextID = 0;
        unlockedSpecies = new HashSet<string>();
        otherUnlocks = new HashSet<string>();
        inProgress = new HashSet<Unlock>();

        Unlock u = new Unlock(nextID++.ToString(), this, new UnlockData(UnlockData.UnlockType.SPECIES, "MonarchButterfly"));
        u.addTask(new PaperTask("GreenPaperTask", Resources.Load("Paper/green") as PaperType, 1000));
        u.addTask(new PaperTask("OrangePaperTask", Resources.Load("Paper/orange") as PaperType, 1000));
        inProgress.Add(u);
    }

    public void moveToInProgress(Unlock u)
    {
        inProgress.Add(u);
    }

    // unlocks a specific item when it is ready
    void unlock(UnlockData u)
    {

        switch(u.getType())
        {
            case UnlockData.UnlockType.SPECIES:
                unlockedSpecies.Add(u.getValue());
                break;
            case UnlockData.UnlockType.TERRARIUM:
                otherUnlocks.Add(u.getValue());
                break;
            default:
                Debug.Log("Unlock type not found! Type: " + u.getType());
                break;
        }

    }

    // returns the ids of all newly unlocked items
    public List<string> checkUnlocks()
    {

        List<string> newUnlocks = new List<string>();
        HashSet<Unlock> toRemove = new HashSet<Unlock>();

        foreach (Unlock u in inProgress)
        {
            Debug.Log(u.getTaskProgress());
            if (u.checkCompletion())
            {
                toRemove.Add(u);
                unlock(u.getValue());
                newUnlocks.Add(u.getID());
            }
        }

        inProgress.ExceptWith(toRemove);

        return newUnlocks;

    }

}

public class UnlockData
{

    public enum UnlockType
    {
        SPECIES = 100,

        TERRARIUM = 200,
    }

    UnlockType type;
    string value;

    public UnlockData(UnlockType type, string value)
    {
        this.type = type;
        this.value = value;
    }

    public UnlockType getType()
    {
        return type;
    }

    public string getValue()
    {
        return value;
    }

}

public class Unlock
{
    string id;
    UnlockData value;
    ProgressionSystem progression;

    HashSet<Task> tasks;
    HashSet<Unlock> children;
    HashSet<Unlock> parents;

    public Unlock(string id, ProgressionSystem progression, UnlockData value) {
        this.id = id;
        this.value = value;
        this.progression = progression;

        tasks = new HashSet<Task>();
        children = new HashSet<Unlock>();
        parents = new HashSet<Unlock>();
    }

    public void addTask(Task t)
    {
        tasks.Add(t);
    }

    public string getID()
    {
        return id;
    }

    public UnlockData getValue()
    {
        return value;
    }

    public void removeParentLock(Unlock u)
    {
        parents.Remove(u);
        if (parents.Count == 0)
            progression.moveToInProgress(this);
    }

    void notifyChildren()
    {
        foreach (Unlock u in children)
        {
            u.removeParentLock(this);
        }
    }

    public bool checkCompletion()
    {

        // check tasks for completion and progress
        bool allComplete = true;
        foreach (Task t in tasks)
        {
            t.checkCompletion();
            allComplete &= t.complete();
        }

        if (allComplete)
        {
            notifyChildren();
        }

        return allComplete;

    }

    public string getTaskProgress()
    {
        string progress = "Unlock ID : " + id + " Value: " + value.getValue();

        int i = 0;
        float progressPercent = 0;
        foreach (Task t in tasks)
        {
            progress += "\n" + "Task " + i + ": " + t.getName() + " [";
            progressPercent = 10 * t.progress();
            for (int j = 1; j <= 10; j++)
            {
                if (j <= progressPercent)
                    progress += "#";
                else
                    progress += "-";
            }
            progress += "]";
        }

        return progress;
    }

}
