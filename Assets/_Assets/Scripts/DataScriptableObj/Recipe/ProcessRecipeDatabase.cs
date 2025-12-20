using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ProcessRecipeDatabase")]
public class ProcessRecipeDatabase : ScriptableObject
{
    public List<ProcessRule> processRules;
}

[System.Serializable]
public class ProcessRule    
{
    public StationType stationType;
    public FoodType inputType;
    public FoodType outputType;
    public float processTime;
    public GameObject resultPrefabs;
}

public enum StationType
{
    EmptyStation, 
    CookingStation,
    CookingSoupStation,
    SliceStation,
    CleanStation,
    GarbageStation,
    ServiceStation,
    BoxStatonCucumber,
    BoxStationBread,
    BoxStationCheese,
    BoxStationLettuce,
    BoxStationMeat,
    BoxStationOnion,
    BoxStationTomato,
    BoxStationMusroom,
    Non
}