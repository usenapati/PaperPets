using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform ItemTemplate;


    private void Awake()
    {
        container = transform.Find("Container");
        ItemTemplate = container.Find("ItemTemplate");
        ItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton("test1", 10, 0);
        CreateItemButton("test2", 100, 1);

    }
//
    //creates the shop buttons, eventually add Sprite to varaibles
    private void CreateItemButton(string name, int cost, int index){
        Transform shop = Instantiate(ItemTemplate, container);
        RectTransform shopTranform = shop.GetComponent<RectTransform>();

        float height = 30f;
        shopTranform.anchoredPosition = new Vector2(0, -height * index);

        shopTranform.Find("name").GetComponent<TextMeshProUGUI>().SetText(name);
        shopTranform.Find("cost").GetComponent<TextMeshProUGUI>().SetText(cost.ToString());
        
        //eventually change name to sprite.
        //shopTranform.Find("name").GetComponent<TextMeshProUGUI>().SetText(name);
    }

    
}
