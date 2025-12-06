using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServiceStation : BaseStation
{
    protected override void PickableObj(PickableObj obj)
    {
        if(obj is not IPlate plate) return;
        if(plate.GetStatePlate() == PlateState.Dirty) return;
        
        List<FoodType> foodTypes = plate.GetFoodTypes();
        if(foodTypes == null) return;
        base.PickableObj(obj);

        TryOrderMatch(foodTypes);
    }

    private bool TryOrderMatch(List<FoodType> food)
    {
        List<OrderUI> orders = OrderManager.Instance.GetOrders();
        if(orders == null) return false;
        foreach(OrderUI order in orders)
        {
            bool isMatch = CompareFoodAccuracy(order.GetFoods(), food);
            if(isMatch) return true;  
        }  
        return false;
    }    

    public bool CompareFoodAccuracy(List<FoodType> foodOders, List<FoodType> foodPlates)
    {
        var countA = foodOders.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());   
        var countB = foodPlates.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        foreach(var key in countA.Keys.Union(countB.Keys))
        {
            int aCount = countA.ContainsKey(key) ? countA[key] : 0;
            int bCount = countB.ContainsKey(key) ? countB[key] : 0;

           if(aCount != bCount) return false;
        }    
        return true;
    }    

}
