using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotSoupObj : PickableObj
{ 
    private CookingRecipeGroup _recipeGroup;

    protected override void Start()
    {
        base.Start();
        _recipeGroup = GetComponent<CookingRecipeGroup>();
    }

    public override void DoSomeThing()
    {
        base.DoSomeThing();
        var pickableObj = _player.GetPickableObj();

        if (pickableObj is FoodObj food)
        {
            var foodType = food.GetDataFood().foodType;

            if(!_recipeGroup.IsValidFood(foodType)) return;  

            var (addSucessfully, cookingRecipe) = _recipeGroup.AddFood(foodType);

            if (addSucessfully) Debug.Log("Food add Sucessfully");
            if (cookingRecipe != null) Debug.Log("Food completed");

        }    
        
    }


}
