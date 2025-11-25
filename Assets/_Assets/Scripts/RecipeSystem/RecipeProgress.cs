using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeProgress
{
    public CookingRecipe cookingRecipe;
    public Dictionary<IngredientRequiment, int> currentAmount = new();

    public RecipeProgress(CookingRecipe recipe)
    {
        cookingRecipe = recipe;
        foreach (var ingredient in recipe.ingredients)
            currentAmount[ingredient] = 0;  
    }

    public bool AddFood(FoodType foodType)
    {
        foreach(var ingredient in cookingRecipe.ingredients)
        {
            if(ingredient.foodType == foodType && currentAmount[ingredient] < ingredient.amount)
            {
                currentAmount[ingredient] += 1;
                return true;
            }
        }
        return false;
    }

    public bool IsCompleted()
    {
        bool hasRequired = false;
        foreach (var ingredient in cookingRecipe.ingredients)
        {
            if(ingredient.isRequied)
            {
                hasRequired = true;
                if (currentAmount[ingredient] < ingredient.amount) return false;  
            }       
        }
       
        if (hasRequired) return true;

        foreach(var ingredient in cookingRecipe.ingredients)
        {
            if(currentAmount[ingredient] ==  ingredient.amount) return true;
        }

        return false;
    }


}
