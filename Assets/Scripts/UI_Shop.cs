using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform ItemTemplate;
    private bool active = false;
    public float spacing = 50f;
    
    public SpeciesType Eagle;
    public SpeciesType Milkweed;
    public SpeciesType Monarch_Butterfly;
    public SpeciesType Oak;
    public SpeciesType Oriole;


    private void Awake()
    {
        container = transform.Find("Container");
        ItemTemplate = container.Find("ItemTemplate");
        ItemTemplate.gameObject.SetActive(active);
    }

    private void GenerateShopValues()
    {
        CreateItemButton(Eagle, 9999999, 0);
        CreateItemButton(Milkweed, 350, 1);
        CreateItemButton(Monarch_Butterfly, 5, 2);
        CreateItemButton(Oak, 15, 3);
        CreateItemButton(Oriole, 120, 4);

    }
//
    //creates the shop buttons, eventually add Sprite to varaibles
    private void CreateItemButton(SpeciesType species, int cost, int index){
        Transform shop = Instantiate(ItemTemplate, container);
        RectTransform shopTranform = shop.GetComponent<RectTransform>();

        float height = spacing;
        shopTranform.anchoredPosition = new Vector2(0, -height * index);

        shopTranform.Find("name").GetComponent<TextMeshProUGUI>().SetText(species.SpeciesName);
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
        
        //eventually change name to sprite.
        //shopTranform.Find("name").GetComponent<TextMeshProUGUI>().SetText(name);
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
        }
        print(active);
        
        

    }

    
}
