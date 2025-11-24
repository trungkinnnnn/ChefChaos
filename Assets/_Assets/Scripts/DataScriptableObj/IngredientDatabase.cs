using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/IngredientDatabase")]
public class IngredientDatabase : ScriptableObject
{
    public List<KitchenItemDatabase> kitchenItems;
}

[System.Serializable]
public class KitchenItemDatabase
{
    public KitchenType kitchenType;
    public List<FoodType> foodTypes;    
}

public enum KitchenType
{
    Plate,
    PlateSoup,
    PotSoup,
}