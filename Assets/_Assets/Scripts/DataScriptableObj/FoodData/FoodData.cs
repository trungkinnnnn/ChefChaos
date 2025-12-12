using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/FoodData"))]
public class FoodData : ScriptableObject
{
    public bool canCut;
    public bool canCook;
    public Sprite sprite;
    public FoodType foodType;
    public float timeCooking;
}

public enum FoodType
{
    Meat,
    MeatSlice,
    MeatChopped,
    MeatMediumCook,
    MeatDoneCook,
    MeatBurntCook,
    Cucumber,
    CucumberSlice,
    Cheese,
    CheeseSlice,
    lettuce,
    lettuceSlice,
    Tomato,
    TomatoSlice,
    Onion,
    OnionSlice,
    Mushroom,
    MushroomSlice,
    Tuna,
    TunaSlice,  
    Rice,
    RiceCook,
    RiceBurntCook,
    Bread,
    Non,
}
