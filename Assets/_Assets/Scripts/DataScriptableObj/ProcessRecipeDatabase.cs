using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ProcessRecipeDatabase")]
public class ProcessRecipeDatabase : ScriptableObject
{
    public List<ProcessGroup> processGroups;
}

[System.Serializable]
public class ProcessGroup
{
    public StationType stationType;
    public List<ProcessRule> processRules;  
}

[System.Serializable]
public class ProcessRule    
{
    public FoodType inputType;
    public FoodType outputType;
    public float processTime;
    public GameObject resultPrefabs;
}

public enum StationType
{
    Slice,
    Fry,
}