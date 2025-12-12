using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotTaskPlanner : MonoBehaviour
{
    private List<FoodType> _foodTypes = new();
    private List<BotStep> _steps = new();

    private void Start()
    {
        StartCoroutine(StartCreatePlanner());
    }

    private IEnumerator StartCreatePlanner()
    {
        yield return new WaitForSeconds(1);
        OrderUI orderUI = OrderManager.Instance.GetOrderFirst();
        _foodTypes = orderUI.GetFoods();
        AddBotStep();
        Print();
    }

    private void AddBotStep()
    {
        _steps.Clear();
        foreach (var foodType in _foodTypes)
        {
            CreatBotStep(foodType);
        }
    }

    private void CreatBotStep(FoodType foodType)
    {
        var processRule = FoodMapManager.Instance.GetInfoFoodMaps(foodType);
        if (processRule == null) return;
        BotStep botStep = new BotStep(processRule.stationType, processRule.outputType);
        _steps.Add(botStep);
        CreatBotStep(processRule.inputType);
    }    

    private void Print()
    {
        foreach(var step in _steps)
        {
            Debug.Log($"{step.targetStation} + {step.requiredFood} + // ");
        }
    }

}


public class BotStep
{
    public StationType targetStation;
    public FoodType requiredFood;

    public BotStep(StationType targetStation, FoodType requiredFood)
    {
        this.targetStation = targetStation;
        this.requiredFood = requiredFood;
    }

}