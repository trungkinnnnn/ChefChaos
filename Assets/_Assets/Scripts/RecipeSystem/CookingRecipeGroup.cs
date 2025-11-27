using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingRecipeGroup : MonoBehaviour
{
    [SerializeField] CookingRecipeDatabase _cookingRecipeData;

    private Dictionary<FoodType, List<IngredientRequiment>> _Lookup = new();
    private List<RecipeProgress> _activeRecipes = new();

    private void Start()
    {
        SetUpData();
        InitRecipeProgress();
    }

    private void SetUpData()
    {
        if (_cookingRecipeData == null) return;

        foreach(CookingRecipe cookingRecipe in _cookingRecipeData.recipes)
        {
            foreach(IngredientRequiment ingredient in cookingRecipe.ingredients)
            {
                if(!_Lookup.ContainsKey(ingredient.foodType))
                {
                    _Lookup[ingredient.foodType] = new List<IngredientRequiment>();
                }
                _Lookup[ingredient.foodType].Add(ingredient);
            }    
        }    
    }    


    // Init
    public void InitRecipeProgress()
    {
        _activeRecipes.Clear();
        foreach(var recipe in _cookingRecipeData.recipes)
        {
            _activeRecipes.Add(new RecipeProgress(recipe));
        }    
    }    


    // Choose ingrediets id follow foodType
    private HashSet<int> GetValidRecipeIDs(FoodType foodType)
    {
        if(!_Lookup.TryGetValue(foodType, out var ingredients)) return null;

        HashSet<int> ids = new();

        foreach(var ing in  ingredients)
            ids.Add(ing.id);

        return ids;
    }


    // Add Food valid
    private bool ApplyFoodToValid(FoodType foodType, HashSet<int> ids)
    {
        bool added = false;

        foreach (var recipeProgress in _activeRecipes)
        {
            if (!ids.Contains(recipeProgress.cookingRecipe.id)) continue;

            if (recipeProgress.AddFood(foodType)) added = true;
        }

        return added;
    }

    // Remove RecipeProgress
    private void FilterActiveRecipes(HashSet<int> ids)
    {
        List<RecipeProgress> toRemoveProgress = new();

        foreach(var recipeProgress in _activeRecipes)
        {
            if(!ids.Contains(recipeProgress.cookingRecipe.id))
                toRemoveProgress.Add(recipeProgress);
        }

        foreach (var recipeProgress in toRemoveProgress)
            _activeRecipes.Remove(recipeProgress);

    }

    private bool ApplyFoodToRecipes(FoodType foodType)
    {
        var validIDs = GetValidRecipeIDs(foodType);
        if(validIDs == null) return false;

        bool added = ApplyFoodToValid(foodType, validIDs);
        if(added) FilterActiveRecipes(validIDs);

        return added;
    }    

    // ===================== Service ======================

    public bool IsValidFood(FoodType foodType)
    {
        return _Lookup.ContainsKey(foodType);
    }    

    public (bool added,CookingRecipe completedRecipe) AddFood(FoodType foodType)
    {
        bool added = ApplyFoodToRecipes(foodType);

        // Check cooking done
        foreach (var recipeProgress in _activeRecipes)
        {
            if(recipeProgress.IsCompleted())
            {
                return (added, recipeProgress.cookingRecipe);   
            } 
        }
        return (added, null);
    } 
        

}
