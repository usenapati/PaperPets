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
    private bool active = false;
    public float spacing = 50f;
    public GameManager gameManager;
    private int count;
    private Dictionary<PaperType, int> paperamounts;

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
    

    int greentext;
    int bluetext;
    int yellowtext;
    int orangetext;
    int browntext;
    int whitetext;
    int redtext;

    private Dictionary<string, bool> isOwned = new Dictionary<string, bool>();

    
    
    // public SpeciesType Eagle;
    // public SpeciesType Milkweed;
    // public SpeciesType Monarch_Butterfly;
    // public SpeciesType Oak;
    // public SpeciesType Oriole;


    private void Awake()
    {
        container = transform.Find("Container");
        ItemTemplate = container.Find("ItemTemplate");
        ItemTemplate.gameObject.SetActive(active);

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

        
    }

    private void Start()
    {
        paperamounts = GameManager.Instance.GetSpendablePaper();
    }

    void Update()
    {
       
        foreach (KeyValuePair<PaperType, int> kv in GameManager.Instance.GetSpendablePaper())
        {
            if(kv.Key.name == "green"){
                greentext = kv.Value;
            }
            if(kv.Key.name == "blue"){
                bluetext = kv.Value;
            }
            if(kv.Key.name == "yellow"){
                yellowtext = kv.Value;
            }
            if(kv.Key.name == "orange"){
                orangetext = kv.Value;
            }
            if(kv.Key.name == "brown"){
                browntext = kv.Value;
            }
            if(kv.Key.name == "white"){
                whitetext = kv.Value;
            }
            if(kv.Key.name == "red"){
                redtext = kv.Value;
            }
        }
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
        foreach(Unlock t in p.inProgress)
        {
            string te = t.getTaskProgress();
            
            foreach(Task t2 in t.getTasks())
            {
                percent = t2.progress();
            }
            
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
        }
        task.text = returningText;

        GameObject[] temp2;
        temp2 = GameObject.FindGameObjectsWithTag("progress");
        foreach(GameObject g in temp2)
        {      
            g.GetComponent<RectTransform>().sizeDelta = new Vector2(percent * 348f, 28f);
            
            // g.GetComponent<RectTransform>().localPosition = new Vector3(
            // g.GetComponent<RectTransform>().localPosition.x + (count * 38.4f), g.GetComponent<RectTransform>().localPosition.y, 0f);
        }
        //GameManager.Instance.getWaterCost().ToString() + "\n" + GameManager.Instance.getLightCost().ToString() + "\n" 
    }

    private void GenerateShopValues()
    {
        count = 0;
        /*foreach (SpeciesType p in Resources.FindObjectsOfTypeAll(typeof(SpeciesType)) as SpeciesType[])
        {
            
            CreateItemButton(p, (int) p.SpeciesCost[0].PaperAmount, count, p.SpeciesCost[0].PaperColor);
         
            
            count++;

        }*/
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
        RectTransform shopTranform = shop.GetComponent<RectTransform>();

        float height = spacing;
        shopTranform.anchoredPosition = new Vector2(0, -height * index);

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

        foreach(KeyValuePair<string, bool> owned in isOwned)
        {
            if(species.SpeciesName == owned.Key)
            {
                shop.Find("background").GetComponent<Image>().color = new Color32(76,85,91,255);
                shop.Find("Owned").GetComponent<TextMeshProUGUI>().SetText("OWNED");
            }
        }

        

      

        

        
        //eventually change name to sprite.
        //shopTranform.Find("name").GetComponent<TextMeshProUGUI>().SetText(name);
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

            isOwned.Add(s.SpeciesName, true);
            GameManager.Instance.setOwned(isOwned);
            
            //change color of background to grey
            shop.Find("background").GetComponent<Image>().color = new Color32(76,85,91,255);
            shop.Find("Owned").GetComponent<TextMeshProUGUI>().SetText("OWNED");

            print(s.SpeciesName + " Added");
        }

        

    }

     public void enableShop()
    {
        
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

            ItemTemplate.gameObject.SetActive(active);
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
        }
        print(active);
        
        

    }

    public void upgradeLight()
    {
        //if(yellowtext >= GameManager.Instance.getLightCost())
        //{
            GameManager.Instance.lightUpgrade();
        //}
    }

    public void upgradeWater()
    {
        //if(bluetext >= GameManager.Instance.getWaterCost())
        //{
            GameManager.Instance.waterUpgrade();
        //}
    }    
}

