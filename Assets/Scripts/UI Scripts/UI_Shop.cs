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

    int greentext;
    int bluetext;
    int yellowtext;
    int orangetext;
    int browntext;
    int whitetext;
    int redtext;

    
    
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

    }

    private void GenerateShopValues()
    {
        count = 0;
        foreach (SpeciesType p in Resources.FindObjectsOfTypeAll(typeof(SpeciesType)) as SpeciesType[])
        {
            
            CreateItemButton(p, p.SpeciesCost[0].PaperAmount, count, p.SpeciesCost[0].PaperColor);
         
            
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
        
        shopTranform.GetComponent<Button>().onClick.AddListener(() => ShopClick(species));

        

      

        

        
        //eventually change name to sprite.
        //shopTranform.Find("name").GetComponent<TextMeshProUGUI>().SetText(name);
    }

    public void ShopClick(SpeciesType s){
        int paperNeeded = s.SpeciesCost[0].PaperAmount;
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
            print(s.SpeciesName + " Added");

            // if(s.SpeciesCost[0].PaperColor.PaperName == "blue")
            // {
            //     bluetext = bluetext - paperNeeded;  
            // }
            // if(s.SpeciesCost[0].PaperColor.PaperName == "brown")
            // {
            //     browntext = browntext - paperNeeded;  
            // }
            // if(s.SpeciesCost[0].PaperColor.PaperName == "green")
            // {
            //     greentext = greentext - paperNeeded;  
            // }
            // if(s.SpeciesCost[0].PaperColor.PaperName == "orange")
            // {
            //     orangetext = orangetext - paperNeeded;  
            // }
            // if(s.SpeciesCost[0].PaperColor.PaperName == "red")
            // {
            //     redtext = redtext - paperNeeded;  
            // }
            // if(s.SpeciesCost[0].PaperColor.PaperName == "white")
            // {
            //     whitetext = whitetext - paperNeeded;  
            // }
            // if(s.SpeciesCost[0].PaperColor.PaperName == "yellow")
            // {
            //     yellowtext = yellowtext - paperNeeded;  
            // }
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
        }
        print(active);
        
        

    }

    
}
