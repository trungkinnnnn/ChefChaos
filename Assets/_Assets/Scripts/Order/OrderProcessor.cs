using System.Collections.Generic;
using UnityEngine;

public class OrderProcessor
{
    private Vector2 _tipRanger;

    public OrderProcessor(Vector2 tipRanger)
    {
        _tipRanger = tipRanger;
    }

    private int CalculateTotalOrderPrice(List<FoodType> orderedFoods, Dictionary<FoodType, InfoFood> lookup)
    {
        int total = 0;
        foreach(var foodType in orderedFoods)
        {
            if(!lookup.ContainsKey(foodType)) Debug.Log("No have food price");
            else total += lookup[foodType].basePrice;
        }
        return total;
    }

    private int GenerateTipBonus()
    {
        return (int)Random.Range(_tipRanger.x, _tipRanger.y);
    }


    // ================ Service =================

    public OrderResult ProcessOrder(List<FoodType> orderFoods, Dictionary<FoodType, InfoFood> lookup)
    {
        int tipBonus = GenerateTipBonus();
        int basePrice = CalculateTotalOrderPrice(orderFoods, lookup);

        return new OrderResult
        {
            BasePrice = basePrice,
            TipBonus = tipBonus,
            TotalEarnings = basePrice + tipBonus
        };

    }
}

public struct OrderResult
{
    public int BasePrice;
    public int TipBonus;
    public int TotalEarnings;
}