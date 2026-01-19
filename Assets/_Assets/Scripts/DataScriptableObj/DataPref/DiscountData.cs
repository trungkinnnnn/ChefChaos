using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/DiscountData"))]
public class DiscountData : ScriptableObject
{
    public List<DiscountLevel> discountLevels;
}


[System.Serializable]
public class DiscountLevel
{
    public int id;
    public float value;
    public int price;
}
