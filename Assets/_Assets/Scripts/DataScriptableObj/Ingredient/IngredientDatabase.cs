using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Data/IngredientDatabase")]
public class IngredientDatabase : ScriptableObject
{
    public List<InfoFood> foodList;
}

[System.Serializable]
public class InfoFood
{
    public FoodType foodType;
    public GameObject prefabFood;
    public int basePrice;
}
