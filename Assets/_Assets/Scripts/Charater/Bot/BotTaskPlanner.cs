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

    private BotExecuteTask _excuteTask;
    private void Start()
    {
        _excuteTask = GetComponent<BotExecuteTask>();
        StartCoroutine(StartCreatePlanner());
    }

    private IEnumerator StartCreatePlanner()
    {
        yield return new WaitForSeconds(1);
        OrderUI orderUI = OrderManager.Instance.GetOrderFirst();
        _foodTypes = orderUI.GetFoods();
        _kichentType = orderUI.GetKitchenType();
        CreateListStep();
        Print();

        _excuteTask.StartActionStep(_steps, _kichentType);
    }

    private void CreateListStep()
    {
        _steps.Clear();

        AddBotStep(StationType.Non, FoodType.Non, _kichentType, 0);
        foreach (var foodType in _foodTypes)
        {
            CreatBotStep(foodType);
            AddBotStep(StationType.Non, foodType, _kichentType, 0);
        }
        AddBotStep(StationType.ServiceStation, FoodType.Non, _kichentType, 0);
    }

    private void CreatBotStep(FoodType foodType)
    {
        var processRules = FoodMapManager.Instance.BuildProcessRules(foodType);
        if (processRules == null) return;
        foreach (var processRule in processRules)
        {
            AddBotStep(processRule.stationType, processRule.outputType, _kichentType, processRule.processTime);
        }    
    }    

    private void AddBotStep(StationType stationType, FoodType foodType, KitchenType kitchenType, float time)
    {
        StepTask stepTask = StepTask.PickUpFood;

        if(stationType != StationType.Non && foodType != FoodType.Non)
        {
            if(time == 0)
            {
                stepTask = StepTask.PickUpFood;
            }
            else
            {
                stepTask = StepTask.ProcessAt;
            }
        }
        else if(foodType != FoodType.Non)
        {
            stepTask = StepTask.DropFood;
        }else
        {
            stepTask = StepTask.StartTask;
        }

        if(stationType == StationType.ServiceStation)
        {
            stepTask = StepTask.EndTask;
        }

        BotStep botStep = new BotStep(stepTask, stationType, foodType,kitchenType, time);
        _steps.Add(botStep);
    }    

    private void Print()
    {
        foreach(var step in _steps)
        {
            Debug.Log($"{step.stepTask} + {step.targetStation} + {step.requiredFood} + {step.kitchenType} + {step.timeCooking} // ");
        }
    }

}

public enum StepTask
{
    PickUpFood,
    ProcessAt,
    DropFood,
    StartTask,
    EndTask
}

