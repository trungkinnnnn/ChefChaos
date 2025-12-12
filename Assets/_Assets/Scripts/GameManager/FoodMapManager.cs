using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMapManager : MonoBehaviour
{
    public static FoodMapManager Instance;

    [SerializeField] List<ProcessRecipeDatabase> _processRecipes = new();
    private Dictionary<FoodType, ProcessRule> _processRules = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        BuildProcessMap();
    }

    private void BuildProcessMap()
    {
        if (_processRecipes == null || _processRecipes.Count == 0) return;
        List<ProcessRule> processRules = new();
        foreach(var processRecipe in _processRecipes)
        {
            processRules.AddRange(processRecipe.processRules);
        }    

        foreach (var processRule in processRules)
        {
            _processRules[processRule.outputType] = processRule;
        }    
    }    

    // ================ Service ===================
    public ProcessRule GetInfoFoodMaps(FoodType foodTypes)
    {
        if (_processRules.ContainsKey(foodTypes)) return _processRules[foodTypes];
        return null;
    }    

}
