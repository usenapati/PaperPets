using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI; 

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform ItemTemplate;
    [SerializeField] Transform taskContainer;
    [SerializeField] GameObject taskTemplate;
    [SerializeField] GameObject unlockTemplate;
    RectTransform tempTransform;
    
    //private Transform progressbar;
    private bool active = false;
    private bool on = false;
    private bool reset = true;
    public float spacing; //= 50f;
    public GameManager gameManager;
    private int count;
    private int tempCount = 1;

    private Dictionary<PaperType, int> paperamounts;
    Dictionary<string, Species> organisms;
    Dictionary<string, GameObject> unlockIndex;

    //texts
    public Text blue;
    public Text yellow;
    public Text orange;
    public Text green;
    public Text red;
    public Text white;
    public Text brown;

    public Text light;
    public Text water;
    public Text task;

    public PaperType o;
    public PaperType b;
    public PaperType br;
    public PaperType gr;
    public PaperType r;
    public PaperType w;
    public PaperType y;
    

    int greentext;
    int bluetext;
    int yellowtext;
    int orangetext;
    int browntext;
    int whitetext;
    int redtext;
    int taskCount = 0;

    private Dictionary<string, bool> isOwned = new Dictionary<string, bool>();

    public AudioClip bought1;
    public AudioClip bought2;
    public AudioClip openShop;
    public AudioSource UIAudio;

    // Birth effect
    public ParticleSystem birthEffect;
    [SerializeField] AudioClip[] birthSounds;

    // public SpeciesType Eagle;
    // public SpeciesType Milkweed;
    // public SpeciesType Monarch_Butterfly;
    // public SpeciesType Oak;
    // public SpeciesType Oriole;


    private void Awake()
    {
      
        
    }

    private void Start()
    {
        container = transform.Find("Shop Title Background").Find("Container");
        ItemTemplate = container.Find("ItemTemplate");
        ItemTemplate.gameObject.SetActive(active);

        //taskContainer = transform.Find("taskContainer");
        //taskTemplate = taskContainer.Find("taskTemplate");
        //taskTemplate.gameObject.SetActive(false);

        unlockIndex = new Dictionary<string, GameObject>(); 
        //progressbar = transform.Find("Progressbar Background");

        isOwned = GameManager.Instance.getOwned();

        GameObject[] temp;
        temp = GameObject.FindGameObjectsWithTag("shop");
        foreach(GameObject g in temp)
        {
            g.GetComponent<Image>().enabled = false;
        }
        temp = GameObject.FindGameObjectsWithTag("shop4");
        foreach(GameObject g in temp)
        {
            g.GetComponent<TextMeshProUGUI>().enabled = false;
        }
        temp = GameObject.FindGameObjectsWithTag("Light");
        foreach(GameObject g in temp)
        {
            g.GetComponent<TextMeshProUGUI>().SetText("");
        }
        temp = GameObject.FindGameObjectsWithTag("Water");
        foreach(GameObject g in temp)
        {
            g.GetComponent<TextMeshProUGUI>().SetText("");
        }
        temp = GameObject.FindGameObjectsWithTag("light&water");
        foreach(GameObject g in temp)
        {
            g.GetComponent<RawImage>().enabled = false;
        }
        temp = GameObject.FindGameObjectsWithTag("infoimage");
        foreach(GameObject g in temp)
        {
            g.GetComponent<RawImage>().enabled = false;
        }
        temp = GameObject.FindGameObjectsWithTag("infotext");
        foreach(GameObject g in temp)
        {
            g.GetComponent<TextMeshProUGUI>().enabled = false;
        }
         temp = GameObject.FindGameObjectsWithTag("infoimage2");
        foreach(GameObject g in temp)
        {
            g.GetComponent<Image>().enabled = false;
        }
        temp = GameObject.FindGameObjectsWithTag("speciesunlockedtext");
        foreach(GameObject g in temp)
        {
            g.GetComponent<TextMeshProUGUI>().enabled = false;
        }
         temp = GameObject.FindGameObjectsWithTag("speciesunlockedimage");
        foreach(GameObject g in temp)
        {
            g.GetComponent<Image>().enabled = false;
        }

        
    }

    void Update()
    {
        GameManager.Instance.getOwned().Clear();
    
        organisms = new Dictionary<string, Species>();
        
        foreach (Species s in GameManager.Instance.getCurrentWorld().getAllSpecies())
        {
            organisms.Add(s.name, s);
        }
        foreach (Species sp in organisms.Values)
        {
            
            GameManager.Instance.getOwned().Add(sp.name, true);
        }
        
       
        foreach (KeyValuePair<PaperType, int> kv in GameManager.Instance.GetSpendablePaper())
        {
            if(kv.Key.name == "green"){
                greentext = kv.Value;
                gr = kv.Key;
            }
            if(kv.Key.name == "blue"){
                bluetext = kv.Value;
                b = kv.Key;
            }
            if(kv.Key.name == "yellow"){
                yellowtext = kv.Value;
                y = kv.Key;
            }
            if(kv.Key.name == "orange"){
                orangetext = kv.Value;
                o = kv.Key;
            }
            if(kv.Key.name == "brown"){
                browntext = kv.Value;
                br = kv.Key;
            }
            if(kv.Key.name == "white"){
                whitetext = kv.Value;
                w = kv.Key;
            }
            if(kv.Key.name == "red"){
                redtext = kv.Value;
                r = kv.Key;
            }
        }
        paperamounts = GameManager.Instance.GetSpendablePaper();
        green.text = greentext.ToString();
        blue.text = bluetext.ToString();
        yellow.text = yellowtext.ToString();
        orange.text = orangetext.ToString();
        brown.text = browntext.ToString();
        white.text = whitetext.ToString();
        red.text = redtext.ToString();

        light.text = GameManager.Instance.getCurrentWorld().getLightLevel().ToString();
        water.text = GameManager.Instance.getCurrentWorld().getWaterLevel().ToString();

        if(active)
        {
            GameObject[] temp;
            temp = GameObject.FindGameObjectsWithTag("Light");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.getLightCost().ToString());
            }
            temp = GameObject.FindGameObjectsWithTag("Water");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.getWaterCost().ToString());
            }
        }

        //gets task and display it
        ProgressionSystem p = GameManager.Instance.getProgression();
        float percent = 0;
        string returningText = "";

        // Transform progress = Instantiate(progressbar);
        // RectTransform progressb = progress.GetComponent<RectTransform>();
        print(unlockIndex.Count);
        foreach(Unlock t in p.inProgress)
        {
            string te = t.getTaskProgress();
            
            foreach (char c in te)
            {
                if(c.ToString() == "["||
                c.ToString() == "]" ||
                c.ToString() == "-"||
                c.ToString() == "#")
                {
                    continue;
                } 
                else{
                    returningText += c;
                }
                
            }
            /*foreach(Task t2 in t.getTasks())
            {
                taskCount++;
                percent = t2.progress();
                CreateTaskList(t, t2.getName(), percent);
                
            }*/

            CreateTaskList(t, "", 0);
            
        }
        taskCount = 0;

        List<string> keysToRemove = new List<string>();
        foreach (KeyValuePair<string, GameObject> kv in unlockIndex)
        {
            if (!GameManager.Instance.getProgression().isIDComplete(kv.Key))
            {
                // destroy unlock
                keysToRemove.Add(kv.Key);
                Destroy(kv.Value);

                GameObject[] temp3;
                temp3 = GameObject.FindGameObjectsWithTag("alert");
                foreach(GameObject g1 in temp3)
                {      
                    g1.GetComponent<TextMeshProUGUI>().SetText("!");
                }
                temp3 = GameObject.FindGameObjectsWithTag("speciesunlockedtext");
                foreach(GameObject g1 in temp3)
                {
                    g1.GetComponent<TextMeshProUGUI>().enabled = true;
                }
                temp3 = GameObject.FindGameObjectsWithTag("speciesunlockedimage");
                foreach(GameObject g1 in temp3)
                {
                    g1.GetComponent<Image>().enabled = true;
                    on = true;
                }
                if(active)
                {
                    GenerateShopValues();
                }
            }
        }
        foreach (string s in keysToRemove)
        {
            unlockIndex.Remove(s);
        }

        //print("0");
        // task.text = returningText;

        // GameObject[] temp2;
        // temp2 = GameObject.FindGameObjectsWithTag("progress");
        // foreach(GameObject g in temp2)
        // {      
        //     g.GetComponent<RectTransform>().sizeDelta = new Vector2(percent * 348f, 28f);
        //     if(percent >= 1)
        //     {
        //         GameObject[] temp3;
        //         temp3 = GameObject.FindGameObjectsWithTag("alert");
        //         foreach(GameObject g1 in temp3)
        //         {      
        //             g1.GetComponent<TextMeshProUGUI>().SetText("!");
        //         }
        //         temp3 = GameObject.FindGameObjectsWithTag("speciesunlockedtext");
        //         foreach(GameObject g1 in temp3)
        //         {
        //             g1.GetComponent<TextMeshProUGUI>().enabled = true;
        //         }
        //         temp3 = GameObject.FindGameObjectsWithTag("speciesunlockedimage");
        //         foreach(GameObject g1 in temp3)
        //         {
        //             g1.GetComponent<Image>().enabled = true;
        //             on = true;
        //         }
                
        //     }
            // GameObject[] colorchange = GameObject.FindGameObjectsWithTag("speciesunlockedimage");
            // if(on)
            // {
            //     foreach(GameObject g1 in colorchange)
            //     {
            //         g1.GetComponent<Image>().color = new Color32((byte)Random.Range(0, 255),(byte)Random.Range(0, 255),(byte)Random.Range(0, 255),255);
            //     }
                
            // }            
            
            // g.GetComponent<RectTransform>().localPosition = new Vector3(
            // g.GetComponent<RectTransform>().localPosition.x + (count * 38.4f), g.GetComponent<RectTransform>().localPosition.y, 0f);
        //}
       
        //GameManager.Instance.getWaterCost().ToString() + "\n" + GameManager.Instance.getLightCost().ToString() + "\n" 
    }

    private void GenerateShopValues()
    {
        int count = 0;
        /*foreach (SpeciesType p in Resources.FindObjectsOfTypeAll(typeof(SpeciesType)) as SpeciesType[])
        {
            
            CreateItemButton(p, (int) p.SpeciesCost[0].PaperAmount, count, p.SpeciesCost[0].PaperColor);
         
            
            count++;

        }*/
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("clone"))
        {
            Destroy(g);
        }
        foreach (string s in GameManager.Instance.getProgression().getUnlockedSpecies())
        {
            SpeciesType p = Resources.Load("Species/" + s) as SpeciesType;
            CreateItemButton(p, (int)p.SpeciesCost[0].PaperAmount, count, p.SpeciesCost[0].PaperColor);
            count++;
        }
        // CreateItemButton(Eagle, 9999999, 0);
        // CreateItemButton(Milkweed, 350, 1);
        // CreateItemButton(Monarch_Butterfly, 5, 2);
        // CreateItemButton(Oak, 15, 3);
        // CreateItemButton(Oriole, 120, 4);

    }
//
    //creates the shop buttons, eventually add Sprite to varaibles
    private void CreateItemButton(SpeciesType species, int cost, int index, PaperType paper){
        Transform shop = Instantiate(ItemTemplate, container);
        shop.gameObject.SetActive(active);
        RectTransform shopTranform = shop.GetComponent<RectTransform>();

/*        float height = spacing;
        shopTranform.localPosition = new Vector2(0, -height * index + ItemTemplate.transform.position.y);*/

        shopTranform.Find("name").GetComponent<TextMeshProUGUI>().SetText(species.SpeciesName);
        //print(species.SpeciesName);
        shopTranform.Find("cost").GetComponent<TextMeshProUGUI>().SetText(cost.ToString());
        shopTranform.Find("foodRequirements").GetComponent<TextMeshProUGUI>().SetText("Food Required:");
        shopTranform.Find("food").GetComponent<TextMeshProUGUI>().SetText(species.FoodRequirements.ToString());
        shopTranform.Find("foodValue").GetComponent<TextMeshProUGUI>().SetText("Food Value:");
        shopTranform.Find("foodVal").GetComponent<TextMeshProUGUI>().SetText(species.FoodValue.ToString());
        shopTranform.Find("ExcessFood").GetComponent<TextMeshProUGUI>().SetText("Excess Food Required:");
        shopTranform.Find("excess").GetComponent<TextMeshProUGUI>().SetText(species.ExcessFoodRequired.ToString());
        shopTranform.Find("LightRequirements").GetComponent<TextMeshProUGUI>().SetText("Light Required:");
        shopTranform.Find("light").GetComponent<TextMeshProUGUI>().SetText(species.LightRequirements.ToString());
        shopTranform.Find("WaterRequirements").GetComponent<TextMeshProUGUI>().SetText("Water Required:");
        shopTranform.Find("water").GetComponent<TextMeshProUGUI>().SetText(species.WaterRequirements.ToString());
        shopTranform.Find("ReproductionChance").GetComponent<TextMeshProUGUI>().SetText("Reproduction Chance:");
        shopTranform.Find("reproduction").GetComponent<TextMeshProUGUI>().SetText(species.ReproductionChance.ToString());
        shopTranform.Find("MaxReproduction").GetComponent<TextMeshProUGUI>().SetText("Max Reproduction:");
        shopTranform.Find("maxReproduction").GetComponent<TextMeshProUGUI>().SetText(species.MaxReproduction.ToString());
        shopTranform.Find("Green").GetComponent<RawImage>().texture = paper.paperTexture;
        
        shopTranform.GetComponent<Button>().onClick.AddListener(() => ShopClick(species, shopTranform));

        foreach(KeyValuePair<string, bool> owned in GameManager.Instance.getOwned())
        {

            if(species.SpeciesName == owned.Key)
            {
                shop.Find("background").GetComponent<Image>().color = new Color32(76,85,91,255);
                shop.Find("Owned").GetComponent<TextMeshProUGUI>().SetText("OWNED");
                count++;
            }
        }

        if(index > count)
        {
            GameObject[] temp2;
            temp2 = GameObject.FindGameObjectsWithTag("alert");
            foreach(GameObject g in temp2)
            {      
                g.GetComponent<TextMeshProUGUI>().SetText("!");
            }
            
        }else
        {
            GameObject[] temp2;
            temp2 = GameObject.FindGameObjectsWithTag("alert");
            foreach(GameObject g in temp2)
            {      
                g.GetComponent<TextMeshProUGUI>().SetText("");
            }
        }
        shopTranform.tag = "clone";
        // shopbuttons.Add(shopTranform, 0); 
        
        //eventually change name to sprite.
        //shopTranform.Find("name").GetComponent<TextMeshProUGUI>().SetText(name);
    }

    private void CreateTaskList(Unlock u, string taskText, float percent){
        
        GameObject[] temp2;
        temp2 = GameObject.FindGameObjectsWithTag("task");
        //print(taskCount);
        /*if(temp2.Length < taskCount || reset)
        {
            
            Transform task1 = Instantiate(taskTemplate, taskContainer);
            RectTransform taskTransform = task1.GetComponent<RectTransform>();
            unlockIndex.Add(taskTransform, taskCount);
            taskTransform.tag = "task";

            float height = 100;
            taskTransform.anchoredPosition = new Vector2(0, -height * (taskCount - 1));

            taskTransform.Find("TaskText").GetComponent<TextMeshProUGUI>().SetText(taskText);
            reset = false;
            
            
        }*/
        if (!unlockIndex.ContainsKey(u.getID()))
        {
            GameObject newUnlock = Instantiate(unlockTemplate, taskContainer);
            newUnlock.GetComponentInChildren<TextMeshProUGUI>().SetText("Unlock: " + u.getValue().getValue());
            unlockIndex.Add(u.getID(), newUnlock);
            int i = 0;
            float height = -100;
            float initOffset = -35;
            foreach (Task t in u.getTasks())
            {
                Transform task1 = Instantiate(taskTemplate, newUnlock.transform).transform;
                task1.Translate(new Vector3(0, initOffset + height * i++));
                RectTransform taskTransform = task1.GetComponent<RectTransform>();
                taskTransform.Find("TaskText").GetComponent<TextMeshProUGUI>().SetText(t.getName());
            }
        }

        /*foreach (KeyValuePair<RectTransform, int> kv in unlockIndex)
        {
            if(kv.Value == taskCount)
            {
                tempTransform = kv.Key;
                if(tempTransform != null)
                { 
                    tempTransform.Find("Progressbar").GetComponent<RectTransform>().sizeDelta = new Vector2(percent * 348f, 28f);
                }
            }
        }*/
        GameObject unlock = unlockIndex[u.getID()];
        for (int i = 0; i < u.getTasks().Count; i++)
        {
            unlock.transform.GetChild(i + 1).Find("Progressbar").GetComponent<RectTransform>().sizeDelta = new Vector2(u.getTasks()[i].progress() * 348f, 28f);
        }
        
        
        
        /*if(percent >= 1)
        {
            unlockIndex.Remove(tempTransform);
            GameObject[] temp3;
            temp3 = GameObject.FindGameObjectsWithTag("alert");
            foreach(GameObject g1 in temp3)
            {      
                g1.GetComponent<TextMeshProUGUI>().SetText("!");
            }
            temp3 = GameObject.FindGameObjectsWithTag("speciesunlockedtext");
            foreach(GameObject g1 in temp3)
            {
                g1.GetComponent<TextMeshProUGUI>().enabled = true;
            }
            temp3 = GameObject.FindGameObjectsWithTag("speciesunlockedimage");
            foreach(GameObject g1 in temp3)
            {
                g1.GetComponent<Image>().enabled = true;
                on = true;
            }
            foreach(GameObject g in temp2)
            {
                Destroy(g);
            }
            
            reset = true;
                
        }*/
        //if (u.checkCompletion)
    
    }

    public void ShopClick(SpeciesType s, RectTransform shop){
        int paperNeeded = (int) s.SpeciesCost[0].PaperAmount;
        int paperHad = 0;
        
        if(s.SpeciesCost[0].PaperColor.PaperName == "blue")
        {
            paperHad = bluetext;  
        }
        if(s.SpeciesCost[0].PaperColor.PaperName == "brown")
        {
            paperHad = browntext;  
        }
        if(s.SpeciesCost[0].PaperColor.PaperName == "green")
        {
            paperHad = greentext;  
        }
        if(s.SpeciesCost[0].PaperColor.PaperName == "orange")
        {
            paperHad = orangetext;  
        }
        if(s.SpeciesCost[0].PaperColor.PaperName == "red")
        {
            paperHad = redtext;  
        }
        if(s.SpeciesCost[0].PaperColor.PaperName == "white")
        {
            paperHad = whitetext;  
        }
        if(s.SpeciesCost[0].PaperColor.PaperName == "yellow")
        {
            paperHad = yellowtext;  
        }

        if(paperHad >= paperNeeded){
            GameManager.Instance.addSpecies(s);
            birthEffect.Play(true);

            //isOwned.Add(s.SpeciesName, true);
            //GameManager.Instance.setOwned(isOwned);
            GameManager.Instance.getOwned().Add(s.SpeciesName, true);

            //change color of background to grey
            shop.Find("background").GetComponent<Image>().color = new Color32(76,85,91,255);
            shop.Find("Owned").GetComponent<TextMeshProUGUI>().SetText("OWNED");

            if(s.SpeciesCost[0].PaperColor.PaperName == "green"){
                paperamounts[gr] = paperHad - paperNeeded;
                
            }
            if(s.SpeciesCost[0].PaperColor.PaperName == "blue"){
                paperamounts[b] = paperHad - paperNeeded;
            }
            if(s.SpeciesCost[0].PaperColor.PaperName == "orange"){
                paperamounts[o] = paperHad - paperNeeded;
            }
            if(s.SpeciesCost[0].PaperColor.PaperName == "red"){
                paperamounts[r] = paperHad - paperNeeded;
            }
            if(s.SpeciesCost[0].PaperColor.PaperName == "yellow"){
                paperamounts[y] = paperHad - paperNeeded;
            }
            if(s.SpeciesCost[0].PaperColor.PaperName == "brown"){
                paperamounts[br] = paperHad - paperNeeded;
            }
            if(s.SpeciesCost[0].PaperColor.PaperName == "white"){
                paperamounts[w] = paperHad - paperNeeded;
            }
            GameManager.Instance.SetSpendablePaper(paperamounts);
            GameObject[] temp2;
            temp2 = GameObject.FindGameObjectsWithTag("alert");
            foreach(GameObject g in temp2)
            {      
                g.GetComponent<TextMeshProUGUI>().SetText("");
            }
            enableShop();
            //UIAudio.PlayOneShot(bought1);
            UIAudio.PlayOneShot(birthSounds[Random.Range(0, birthSounds.Length)]);
        }
    }

     public void enableShop()
    {
        UIAudio.PlayOneShot(openShop);
        if(active){
            active = false;
            
            GameObject[] temp;
            temp = GameObject.FindGameObjectsWithTag("shop");
            foreach(GameObject g in temp)
            {
                g.GetComponent<Image>().enabled = false;
            }

            temp = GameObject.FindGameObjectsWithTag("shop3");
            foreach(GameObject g in temp)
            {
                g.GetComponent<RawImage>().enabled = false;
            }
            temp = GameObject.FindGameObjectsWithTag("shop2");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText("");
            }
             temp = GameObject.FindGameObjectsWithTag("shop4");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().enabled = false;
            }
            temp = GameObject.FindGameObjectsWithTag("Light");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText("");
            }
            temp = GameObject.FindGameObjectsWithTag("Water");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText("");
            }
            temp = GameObject.FindGameObjectsWithTag("light&water");
            foreach(GameObject g in temp)
            {
                g.GetComponent<RawImage>().enabled = false;
            }
            temp = GameObject.FindGameObjectsWithTag("exit");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText("Shop");
            }
            temp = GameObject.FindGameObjectsWithTag("clone");
            foreach(GameObject g in temp)
            {
                Destroy(g);
            }
            // foreach(KeyValuePair<RectTransform, int> t in shopbuttons)
            // {
            //     
            // }
            
        } 
        else{
            active = true;

            GameObject[] temp;
            temp = GameObject.FindGameObjectsWithTag("shop");
            foreach(GameObject g in temp)
            {
                g.GetComponent<Image>().enabled = true;
            }
             temp = GameObject.FindGameObjectsWithTag("shop3");
            foreach(GameObject g in temp)
            {
                g.GetComponent<RawImage>().enabled = true;
            }

            //ItemTemplate.gameObject.SetActive(active);
            GenerateShopValues();

            temp = GameObject.FindGameObjectsWithTag("shop4");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().enabled = true;
            }
            temp = GameObject.FindGameObjectsWithTag("Light");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.getLightCost().ToString());
            }
            temp = GameObject.FindGameObjectsWithTag("Water");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.getWaterCost().ToString());
            }
            temp = GameObject.FindGameObjectsWithTag("light&water");
            foreach(GameObject g in temp)
            {
                g.GetComponent<RawImage>().enabled = true;
            }
            temp = GameObject.FindGameObjectsWithTag("exit");
            foreach(GameObject g in temp)
            {
                g.GetComponent<TextMeshProUGUI>().SetText("Close");
            }
            
            GameObject[] temp3;
            temp3 = GameObject.FindGameObjectsWithTag("speciesunlockedtext");
            foreach(GameObject g1 in temp3)
            {
                g1.GetComponent<TextMeshProUGUI>().enabled = false;
            }
            temp3 = GameObject.FindGameObjectsWithTag("speciesunlockedimage");
            foreach(GameObject g1 in temp3)
            {
                g1.GetComponent<Image>().enabled = false;
            }
            
        }
        //print(active);
        
        

    }

    public void upgradeLight()
    {
        int paperHad = yellowtext;
        int paperNeeded = GameManager.Instance.getLightCost();
      
        if(yellowtext >= paperNeeded)
        {
            GameManager.Instance.lightUpgrade();
            paperamounts[y] = paperHad - paperNeeded;
        }
    }

    public void upgradeWater()
    {
        int paperHad = bluetext;
        int paperNeeded = GameManager.Instance.getWaterCost();

        if(bluetext >= paperNeeded)
        {
            GameManager.Instance.waterUpgrade();
            paperamounts[b] = paperHad - paperNeeded;
        }
    }    
}

