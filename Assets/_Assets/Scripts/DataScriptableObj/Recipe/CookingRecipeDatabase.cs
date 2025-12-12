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
    public Sprite sprite;
    public GameObject resultPrefab; 
    public KitchenType type;
    public List<IngredientRequiment> ingredients;
    public int maxTotalFood;
    public float timeCookDone = 5f;
}

[System.Serializable]
public class IngredientRequiment
{
    public int id;  
    public FoodType foodType;
    public Sprite foodSprite;
    public int amount;
    public bool isRequied;
}