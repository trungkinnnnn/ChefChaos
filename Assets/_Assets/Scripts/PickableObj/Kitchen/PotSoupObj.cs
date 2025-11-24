using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotSoupObj : PickableObj
{
    [SerializeField] KitchenType kitchenType;
    public override void DoSomeThing()
    {
        base.DoSomeThing();
        if(_player.GetPickableObj() is FoodObj foodObj && 
            RecipeManager.Instance.IsvalidIngredient(kitchenType, foodObj.GetDataFood().foodType))
        {
            
        }    
        
    }


}
