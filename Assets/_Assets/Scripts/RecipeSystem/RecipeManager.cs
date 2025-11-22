using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    [SerializeField] ProcessRecipeDatabase _processRecipeDatabase;
    private Dictionary<StationType, List<ProcessRule>> _listProcess = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;    
            DontDestroyOnLoad(gameObject);  
        }else
        {
            Destroy(gameObject);    
        }    

    }

    private void Start()
    {
        SetUpdata();
    }

    private void SetUpdata()
    {
        if(_processRecipeDatabase == null) return;
        _listProcess.Clear();   

        foreach(var list in _processRecipeDatabase.processGroups)
        {
            _listProcess.Add(list.stationType, list.processRules);
        }    
    }    


    // =============== Service =================
    public ProcessRule GetProcessRule(StationType stationType, FoodType foodType)
    {
        if(_listProcess.TryGetValue(stationType, out List<ProcessRule> processRules))
        {
            foreach(ProcessRule processRule in processRules)
            {
                if(processRule.inputType == foodType) return processRule;
            }    
        }    
        return null;
    }    

}
