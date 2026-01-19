using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/ProgessData"))]
public class ProgessData : ScriptableObject
{
    public List<DayLevel> daylevels;
}

[System.Serializable]
public class DayLevel
{
    public int id;
    public int dayLevel;
    public CookingRecipeDatabase database;
}