using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SystemViewCircleData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] GameObject infoBox;
    [SerializeField] SpriteRenderer bg;
    [SerializeField] TextMeshPro sName;
    [SerializeField] TextMeshPro food;
    [SerializeField] TextMeshPro water;
    [SerializeField] TextMeshPro paper;

    SystemViewCameraControls test;
    Species species;

    public void setSpecies(Species s)
    {
        this.species = s;
        SpeciesVisualData sd = Resources.Load("Visuals/" + s.name.Replace(" ", "")) as SpeciesVisualData;
        bg.color = sd == null ? Color.black : sd.speciesColor;
        sName.text = s.name;
        food.text = s.requiresFood() ? "Food Requirements: " + s.getRequiredFood() : "Light Requirements: " + s.getRequiredLight();
        water.text += s.getRequiredWater();
        foreach (PaperValue pv in s.producedPaper())
        {
            paper.text += pv.PaperColor.PaperName + ": " + pv.PaperAmount + "\n";
        }
    }

    public void OnPointerEnter(PointerEventData p)
    {
        infoBox.SetActive(true);
        infoBox.transform.localScale = new Vector3(GameManager.Instance.getUIZoom()/170, GameManager.Instance.getUIZoom()/170, 1);
        //infoBox.transform.position = new Vector3(0, GameManager.Instance.getUIZoom()/10, 0);
  
        print("enter");
    }

    public void OnPointerExit(PointerEventData p)
    {
        infoBox.SetActive(false);
        print("exit");
    }

}
