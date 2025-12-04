using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = ("Data/TypeCook"))]
public class TypeCook : ScriptableObject
{
    public List<IconCook> icons;    
}

[System.Serializable]
public class IconCook
{
    public FoodType foodType;
    public Sprite _spriteCook;
}