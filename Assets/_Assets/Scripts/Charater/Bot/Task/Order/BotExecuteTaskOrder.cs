using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotExecuteTaskOrder
{
    private float _timeDelay;
    private BotContext _botContext;
    public BotExecuteTaskOrder(BotContext botContext, float timeDelay = 0.7f)
    {
        _timeDelay = timeDelay;
        _botContext = botContext;
    }
    private IEnumerator ExecuteStep(List<BotStep> steps, KitchenType kitchenType)
    {
        foreach (BotStep step in steps)
        {
            yield return HandleDoingTask(step, kitchenType);
        }
    }

    private IEnumerator HandleDoingTask(BotStep step, KitchenType kitchenType)
    {
        switch (step.stepTask)
        {
            case StepTask.StartTask:
                StartTask startTask = new StartTask(kitchenType);
                yield return startTask.Execute(_botContext, _timeDelay);
                Debug.Log("Done StartTask");
                break;
            case StepTask.EndTask:
                EndTask endTask = new EndTask(kitchenType);
                yield return endTask.Execute(_botContext, _timeDelay);
                Debug.Log("Done EndTask");
                break;
            case StepTask.PickUpFood:
                PickupFoodTask pickFood = new PickupFoodTask(step);
                yield return pickFood.Execute(_botContext, _timeDelay);
                Debug.Log("Done PickUpFood");
                break;
            case StepTask.DropFood:
                DropFoodTask dropFood = new DropFoodTask(step);
                yield return dropFood.Execute(_botContext, _timeDelay);
                Debug.Log("Done DropFood");
                break;
            case StepTask.ProcessAt:
                ProcessAtStationTask processAt = new ProcessAtStationTask(step, _timeDelay);
                yield return processAt.Execute(_botContext, _timeDelay);
                Debug.Log("Done ProcessAt");
                break;

        }
    }

    // =============== Service ==============
    public IEnumerator StartActionStep(List<BotStep> steps, KitchenType kitchenType)
    {
        yield return ExecuteStep(steps, kitchenType);
    }

}
