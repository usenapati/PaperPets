using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Xml;

[JsonObject(MemberSerialization.OptIn)]
public class ProgressionSystem
{

    // for building the initial tree of tasks
    Dictionary<string, Unlock> idToUnlock;
    Dictionary<string, List<string>> idToParents;
    Dictionary<string, List<string>> idToChildren;
    HashSet<string> unlockIgnores;

    // next id of a newly created task
    [JsonProperty] int nextID;

    // unlocked items
    [JsonProperty] HashSet<string> unlockedSpecies;
    [JsonProperty] HashSet<string> otherUnlocks;

    // in progress items
    public HashSet<Unlock> inProgress;
    HashSet<Unlock> inProgressTemp;
    HashSet<string> inProgressIDs;

    public ProgressionSystem()
    {
        nextID = 0;
        unlockedSpecies = new HashSet<string>();
        otherUnlocks = new HashSet<string>();
        inProgress = new HashSet<Unlock>();
        inProgressTemp = new HashSet<Unlock>();

        idToUnlock = new Dictionary<string, Unlock>();
        idToParents = new Dictionary<string, List<string>>();
        idToChildren = new Dictionary<string, List<string>>();
        unlockIgnores = new HashSet<string>();
        inProgressIDs = new HashSet<string>();
    }

    public void setup()
    {
        loadXMLData("XML/Progression");

        // remove extraneous unlocks
        foreach (KeyValuePair<string, Unlock> kv in idToUnlock)
        {
            // make sure unlock has not been completed
            if (unlockedSpecies.Contains(kv.Value.getValue().getValue()) || otherUnlocks.Contains(kv.Value.getValue().getValue()))
            {
                Debug.Log("Added " + kv.Key + " to ignore list");
                unlockIgnores.Add(kv.Key);
            }
        }

        //Debug.Log("ignore list size: " + unlockIgnores.Count);

        // ensure that each unlock is linked up appropriately
        foreach (KeyValuePair<string, Unlock> kv in idToUnlock)
        {

            if (unlockIgnores.Contains(kv.Key)) continue;

            // set up parents
            foreach (string s in idToParents[kv.Key])
            {
                if (unlockIgnores.Contains(s)) continue;
                kv.Value.addParent(idToUnlock[s]);
            }

            if (kv.Value.parentCount() == 0)
            {
                inProgress.Add(kv.Value);
                inProgressIDs.Add(kv.Value.getID());
            }

            // set up children
            foreach (string s in idToChildren[kv.Key])
            {
                if (unlockIgnores.Contains(s)) continue;
                kv.Value.addChild(idToUnlock[s]);
            }

        }
        unlockedSpecies.Add("Milkweed");
        unlockedSpecies.Add("Aphid");
        //unlockAll();
    }

    public HashSet<string> getUnlocks()
    {
        HashSet<string> totalUnlocks = new HashSet<string>();
        totalUnlocks.UnionWith(unlockedSpecies);
        totalUnlocks.UnionWith(otherUnlocks);
        return totalUnlocks;
    }

    public HashSet<string> getUnlockedSpecies()
    {
        return new HashSet<string>(unlockedSpecies);
    }

    public bool isIDComplete(string id)
    {
        return inProgressIDs.Contains(id);
    }

    public void moveToInProgress(Unlock u)
    {
        inProgressTemp.Add(u);
        inProgressIDs.Add(u.getID());
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
            //Debug.Log(u.getTaskProgress());
            if (u.checkCompletion())
            {
                toRemove.Add(u);
                unlock(u.getValue());
                newUnlocks.Add(u.getID());
                inProgressIDs.Remove(u.getID());
            }
        }

        inProgress.ExceptWith(toRemove);
        inProgress.UnionWith(inProgressTemp);
        inProgressTemp.Clear();

        return newUnlocks;

    }

    private List<string> readParentsChildren(XmlReader reader)
    {
        List<string> ids = new List<string>();

        while(reader.Read())
        {
            if (reader.HasAttributes) ids.Add(reader.GetAttribute("id"));
        }

        reader.Close();
        Debug.Log("IDs in list: " + ids);
        return ids;
    }

    private Task readTasks(XmlReader reader)
    {
        //List<Task> tasks = new List<Task>();
        Task task = null;

        while (reader.Read())
        {
            if (!reader.HasAttributes) continue;
            switch (reader.GetAttribute("type"))
            {
                case "PaperTask":
                    reader.Read();
                    reader.Read();
                    string name = reader.GetAttribute("name");
                    reader.Read();
                    reader.Read();
                    string color = reader.GetAttribute("color");
                    reader.Read();
                    reader.Read();
                    float amount = float.Parse(reader.GetAttribute("amount"));
                    task = new PaperTask(name, Resources.Load("Paper/" + color) as PaperType, amount);
                    break;
                case "SpeciesTask":
                    reader.Read();
                    reader.Read();
                    string sName = reader.GetAttribute("name");
                    reader.Read();
                    reader.Read();
                    string speciesType = reader.GetAttribute("species");
                    task = new SpeciesTask(sName, speciesType);
                    break;
                default:
                    Debug.LogWarning("Task type not found of type: " + reader.GetAttribute("type"));
                    break;
            }
        }

        reader.Close();
        return task;
    }

    private void readUnlock(string id, XmlReader reader)
    {
        Debug.Log("New unlock with id: " + id);
        string type = "";
        string value = "";
        List<string> parents = new List<string>();
        List<string> children = new List<string>();
        List<Task> tasks = new List<Task>();

        /*reader.Read();
        reader.Read();
        reader.Read();
        Debug.Log("id: " + id + " node name: " + reader.Name);*/
        while (reader.Read())
        {
            switch (reader.Name)
            {
                case "unlockdata":
                    type = reader.GetAttribute("type");
                    value = reader.GetAttribute("value");
                    break;
                case "parents":
                    parents = readParentsChildren(reader.ReadSubtree());
                    break;
                case "children":
                    children = readParentsChildren(reader.ReadSubtree());
                    break;
                case "task":
                    Task task = readTasks(reader.ReadSubtree());
                    if (task != null) tasks.Add(task);
                    break;
            }
        }

        reader.Close();
        UnlockData ud = null;

        switch (type)
        {
            case "SPECIES":
                ud = new UnlockData(UnlockData.UnlockType.SPECIES, value);
                break;
            case "TERRARIUM":
                ud = new UnlockData(UnlockData.UnlockType.TERRARIUM, value);
                break;
            default:
                Debug.LogWarning("Unlock type not found of type: " + type);
                break;
        }

        Unlock u = new Unlock(id, this, ud);
        //Debug.Log("Tasks to add to Unlock " + id + ": " + tasks.Count);
        foreach (Task t in tasks)
        {
            u.addTask(t);
        }
        idToUnlock.Add(id, u);
        idToParents.Add(id, parents);
        idToChildren.Add(id, children);
    }

    private void loadXMLData(string path)
    {

        // load the proper xml file
        TextAsset file = Resources.Load(path) as TextAsset;
        MemoryStream m = new MemoryStream(file.bytes);
        XmlReader reader = XmlReader.Create(m);

        while (reader.Read())
        {
            // check each of the stages
            if (reader.IsStartElement())
            {
                switch (reader.Name)
                {
                    case "unlock":
                        readUnlock(reader.GetAttribute("id"), reader.ReadSubtree());
                        break;
                }
            }
        }

    }

    private void unlockAll()
    {
        foreach (SpeciesType s in Resources.LoadAll<SpeciesType>("Species"))
        {
            unlockedSpecies.Add(s.SpeciesName);
        }
        foreach (PaperType p in Resources.LoadAll<PaperType>("Paper")) {
            GameManager.Instance.GetSpendablePaper()[p] += 10000;
        }
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

    public void addParent(Unlock u)
    {
        parents.Add(u);
    }

    public void addChild(Unlock u)
    {
        children.Add(u);
    }

    public int parentCount()
    {
        return parents.Count;
    }

    public int childCount()
    {
        return children.Count;
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

    public List<Task> getTasks()
    {
        return new List<Task>(tasks);
    }

}
