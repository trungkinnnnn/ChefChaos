using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTaskPlannerOrder
{
    private List<BotStep> _steps;
    private KitchenType _kitchenType;
    private List<BotStep> CreateListStep(List<FoodType> foodTypes, KitchenType kitchenType)
    {
        _steps.Clear();

        AddBotStep(StationType.Non, FoodType.Non, kitchenType, 0);
        foreach (var foodType in foodTypes)
        {
            CreatBotStep(foodType, kitchenType);
            AddBotStep(StationType.Non, foodType, kitchenType, 0);
        }
        AddBotStep(StationType.ServiceStation, FoodType.Non, kitchenType, 0);
        return _steps;
    }

    private void CreatBotStep(FoodType foodType, KitchenType kitchenType)
    {
        var processRules = FoodMapManager.Instance.BuildProcessRules(foodType);
        if (processRules == null) return;
        foreach (var processRule in processRules)
        {
            AddBotStep(processRule.stationType, processRule.outputType, kitchenType, processRule.processTime);
        }
    }

    private void AddBotStep(StationType stationType, FoodType foodType, KitchenType kitchenType, float time)
    {
        StepTask stepTask = StepTask.PickUpFood;

        if (stationType != StationType.Non && foodType != FoodType.Non)
        {
            if (time == 0)
            {
                stepTask = StepTask.PickUpFood;
            }
            else
            {
                stepTask = StepTask.ProcessAt;
            }
        }
        else if (foodType != FoodType.Non)
        {
            stepTask = StepTask.DropFood;
        }
        else
        {
            stepTask = StepTask.StartTask;
        }

        if (stationType == StationType.ServiceStation)
        {
            stepTask = StepTask.EndTask;
        }

        BotStep botStep = new BotStep(stepTask, stationType, foodType, kitchenType, time);
        _steps.Add(botStep);
    }

    private void Print()
    {
        Debug.Log("step count" + _steps.Count);
        foreach (var step in _steps)
        {
            Debug.Log($"{step.stepTask} + {step.targetStation} + {step.requiredFood} + {step.kitchenType} + {step.timeCooking} // ");
        }
    }

    // ================ Service ================
    public List<BotStep> StartCreatePlanner()
    {
        _steps = new();
        OrderUI orderUI = OrderManager.Instance.GetOrderFirst();
        var foodTypes = orderUI.GetFoods();
        _kitchenType = orderUI.GetKitchenType();
        return CreateListStep(foodTypes, _kitchenType);
    }

    public KitchenType GetKitchenType() => _kitchenType;

}
