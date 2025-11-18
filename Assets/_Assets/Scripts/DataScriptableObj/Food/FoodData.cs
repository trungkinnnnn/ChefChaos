using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/FoodData")]
public class FoodData : ScriptableObject
{
    public string nameFood;
    public Sprite spriteFood;
    public Material materialDefault;
    public Material materialHighlight;
}
