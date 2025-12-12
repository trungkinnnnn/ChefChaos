using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationRecipeManager : MonoBehaviour
{
    [SerializeField] ProcessRecipeDatabase _processRecipeDatabase;
    private Dictionary<FoodType, ProcessRule> _listProcess = new();

    private void Start()
    {
        SetUpdataProcess();
    }

    private void SetUpdataProcess()
    {
        if(_processRecipeDatabase == null) return;
        _listProcess.Clear();   

        foreach(var processRule in _processRecipeDatabase.processRules)
        {
            _listProcess.Add(processRule.inputType, processRule);
        }    
    }   

    // =============== Service =================
    public ProcessRule GetProcessRule(FoodType foodType)
    {
        if(_listProcess.TryGetValue(foodType, out ProcessRule processRule)) return processRule;  
        return null;
    }    
}
