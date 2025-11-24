using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CookingRecipeDatabase")]
public class CookingRecipeDatabase : ScriptableObject
{
    public List<CookingRecipe> recipes;
}

[System.Serializable]
public class CookingRecipe
{
    public string disName;
    public GameObject resultPrefab;
    public KitchenType kitchenType;
    public List<IngredientRequiment> ingredients;
}

[System.Serializable]
public class IngredientRequiment
{
    public List<FoodType> foodType;
    public int amount;
    public bool isRequied;
}