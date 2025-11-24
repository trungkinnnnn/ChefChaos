using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    [SerializeField] ProcessRecipeDatabase _processRecipeDatabase;
    [SerializeField] IngredientDatabase _ingredientDatabase;
    [SerializeField] CookingRecipeDatabase _cookingRecipeDatabase;

    private Dictionary<StationType, List<ProcessRule>> _listProcess = new();
    private Dictionary<KitchenType, List<FoodType>> _listFood = new();

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
        SetUpdataProcess();
        SetUpdataIngredient();
    }

    private void SetUpdataProcess()
    {
        if(_processRecipeDatabase == null) return;
        _listProcess.Clear();   

        foreach(var list in _processRecipeDatabase.processGroups)
        {
            _listProcess.Add(list.stationType, list.processRules);
        }    
    }    

    private void SetUpdataIngredient()
    {
        if(_ingredientDatabase == null) return;
        _listFood.Clear();  

        foreach(var list in _ingredientDatabase.kitchenItems)
        {
            _listFood.Add(list.kitchenType, list.foodTypes);
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

    public bool IsvalidIngredient(KitchenType kitchenType, FoodType foodType)
    {
        if(_listFood.TryGetValue(kitchenType, out List<FoodType> kitchenTypes))
        {
            foreach(FoodType food in kitchenTypes)
            {
                if(food == foodType) return true;
            }    
        }    
        return false;
    }    

}
