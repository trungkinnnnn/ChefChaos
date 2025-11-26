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
    public int id;
    public string disName;
    public GameObject resultPrefab;
    public List<IngredientRequiment> ingredients;
}

[System.Serializable]
public class IngredientRequiment
{
    public int id;  
    public FoodType foodType;
    public int amount;
    public bool isRequied;
}