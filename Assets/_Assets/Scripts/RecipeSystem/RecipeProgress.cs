using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeProgress
{
    public CookingRecipe cookingRecipe;
    public Dictionary<IngredientRequiment, int> currentAmount = new();
    private int _totalFoodAmount = 0;   

    public RecipeProgress(CookingRecipe recipe)
    {
        _totalFoodAmount = 0;
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
                _totalFoodAmount += 1;
                currentAmount[ingredient] += 1;
                return true;
            }
        }
        return false;
    }

    public bool IsCompleted()
    {
        bool hasRequired = false;

        if(_totalFoodAmount == cookingRecipe.maxTotalFood) return true;

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
