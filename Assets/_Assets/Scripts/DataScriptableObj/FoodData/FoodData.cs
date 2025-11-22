using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/FoodData"))]
public class FoodData : ScriptableObject
{
    public bool canCut;
    public bool canCook;
    public FoodType foodType;
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
}
