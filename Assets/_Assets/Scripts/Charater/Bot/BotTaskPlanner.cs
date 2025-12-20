using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotTaskPlanner : MonoBehaviour
{
    private List<FoodType> _foodTypes = new();
    private List<BotStep> _steps = new();
    private KitchenType _kichentType;
    private void Start()
    {
        StartCoroutine(StartCreatePlanner());
    }

    private IEnumerator StartCreatePlanner()
    {
        yield return new WaitForSeconds(1);
        OrderUI orderUI = OrderManager.Instance.GetOrderFirst();
        _foodTypes = orderUI.GetFoods();
        _kichentType = orderUI.GetKitchenType();
        AddBotStep();
        Print();
    }

    private void AddBotStep()
    {
        foreach (var foodType in _foodTypes)
        {
            CreatBotStep(foodType);
            BotStep botStep = new BotStep(StationType.Non, foodType, _kichentType, 0);
            _steps.Add(botStep);
        }
        BotStep botStep2 = new BotStep(StationType.ServiceStation, FoodType.Non, _kichentType, 0);
        _steps.Add(botStep2);
    }

    private void CreatBotStep(FoodType foodType)
    {
        var processRules = FoodMapManager.Instance.BuildProcessRules(foodType);
        if (processRules == null) return;
        foreach (var processRule in processRules)
        {
            BotStep botStep = new BotStep(processRule.stationType, processRule.outputType, _kichentType, processRule.processTime);
            _steps.Add(botStep);
        }    
    }    

    private void Print()
    {
        foreach(var step in _steps)
        {
            Debug.Log($"{step.targetStation} + {step.requiredFood} + {step.kitchenType} + {step.timeCooking} // ");
        }
    }

}
