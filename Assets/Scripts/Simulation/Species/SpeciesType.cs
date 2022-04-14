using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Species", menuName = "Species/New Species", order = 2), JsonObject(MemberSerialization.Fields)]
public class SpeciesType : ScriptableObject
{

    // species specific requirements
    public string SpeciesName { get { return speciesName; } private set { speciesName = value; } }
    [SerializeField] private string speciesName;
    public float FoodRequirements { get { return foodRequirements; } private set { foodRequirements = value; } }
    [SerializeField] private float foodRequirements;
    public float FoodValue { get { return foodValue; } private set { foodValue = value; } }
    [SerializeField] private float foodValue;
    public float ExcessFoodRequired { get { return excessFoodRequired; } private set { excessFoodRequired = value; } }
    [SerializeField] private float excessFoodRequired;
    public float WaterRequirements { get { return waterRequirements; } private set { waterRequirements = value; } }
    [SerializeField] private float waterRequirements;
    public float LightRequirements { get { return lightRequirements; } private set { lightRequirements = value; } }
    [SerializeField] private float lightRequirements;
    public float ReproductionChance { get { return reproductionChance; } private set { reproductionChance = value; } }
    [SerializeField] private float reproductionChance;
    public float MaxReproduction { get { return maxReproduction; } private set { maxReproduction = value; } }
    [SerializeField] private float maxReproduction;

    // things species is, lives in, eats, and preferred biomes
    public List<string> Tags { get { return tags; } private set { tags = value; } }
    [SerializeField] private List<string> tags;
    public bool RequiresHabitat { get { return requiresHabitat; } private set { requiresHabitat = value; } }
    [SerializeField] private bool requiresHabitat;
    public List<string> Habitats { get { return habitats; } private set { habitats = value; } }
    [SerializeField] private List<string> habitats;
    public bool RequiresFood { get { return requiresFood; } private set { requiresFood = value; } }
    [SerializeField] private bool requiresFood;
    public List<string> Foods { get { return foods; } private set { foods = value; } }
    [SerializeField] private List<string> foods;
    //List<BiomeType> favoredBiomes;s

    // paper species costs to add
    public List<PaperValue> SpeciesCost { get { return speciesCost; } private set { speciesCost = value; } }
    [SerializeField] private List<PaperValue> speciesCost;

    // paper species will produce
    public List<PaperValue> SpeciesProduce { get { return speciesProduce; } private set { speciesProduce = value; } }
    [SerializeField] private List<PaperValue> speciesProduce;

    // biome information
    public float IdealTemperature { get { return idealTemperature; } private set { idealTemperature = value; } }
    [SerializeField] private float idealTemperature;
    public float IdealHumidity { get { return idealHumidity; } private set { idealHumidity = value; } }
    [SerializeField] private float idealHumidity;
    public float IdealWindSpeed { get { return idealWindSpeed; } private set { idealWindSpeed = value; } }
    [SerializeField] private float idealWindSpeed;
    public float BiomeWeight { get { return biomeWeight;  } private set { biomeWeight = value; } }
    [SerializeField] private float biomeWeight;
    public float[] TempRange {  get { return tempRange; } private set { tempRange = value; } }
    [SerializeField] float[] tempRange;
    public float[] HumidityRange { get { return humidityRange; } private set { humidityRange = value; } }
    [SerializeField] private float[] humidityRange;

    //species sprite
    public Sprite sprite { get { return speciesSprite; } private set { speciesSprite = value; } }
    [SerializeField] private Sprite speciesSprite;

}

[System.Serializable]
public struct PaperValue
{ 
    public PaperValue(PaperType paperColor, float paperValue)
    {
        this.paperColor = paperColor;
        this.paperValue = paperValue;
    }
    public PaperType PaperColor { get { return paperColor; } private set { paperColor = value; } }
    [SerializeField] PaperType paperColor;
    public float PaperAmount { get { return paperValue;  } private set { paperValue = value; } }
    [SerializeField] float paperValue;
}
