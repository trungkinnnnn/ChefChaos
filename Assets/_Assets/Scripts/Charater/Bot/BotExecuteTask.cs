using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotExecuteTask : MonoBehaviour
{
    private float _timeDelay = 0.7f;
    private BotContext _botContext;

    private BotMovement _movment;
    private PlayerInteraction _interaction;
    private void Start()
    {
        _movment = GetComponent<BotMovement>();
        _interaction = GetComponent<PlayerInteraction>();
        _botContext = new BotContext(_movment, _interaction, transform);
    }

    private IEnumerator ExecuteStep(List<BotStep> steps, KitchenType kitchenType)
    {
        foreach (BotStep step in steps)
        {
            yield return HandleDoingTask(step, kitchenType);
            //yield return new WaitForSeconds(_timeDelay);
        }
    }    

    private IEnumerator HandleDoingTask(BotStep step, KitchenType kitchenType)
    {
        switch(step.stepTask)
        {
            case StepTask.StartTask:
                StartTask startTask = new StartTask(kitchenType);
                yield return startTask.Execute(_botContext, _timeDelay);
                Debug.Log("Done StartTask");
                break;
            //case StepTask.EndTask:
            //    EndTask endTask = new EndTask(kitchenType);
            //    yield return endTask.Execute(_botContext, _timeDelay);
            //    Debug.Log("Done EndTask");
            //    break;
            case StepTask.PickUpFood:
                PickupFoodTask pickFood = new PickupFoodTask(step);
                yield return pickFood.Execute(_botContext, _timeDelay);
                Debug.Log("Done PickUpFood");
                break;
            //case StepTask.DropFood:
            //    DropFoodTask dropFood = new DropFoodTask(step);
            //    yield return dropFood.Execute(_botContext, _timeDelay);
            //    Debug.Log("Done DropFood");
            //    break;
            //case StepTask.ProcessAt:
            //    ProcessAtStationTask processAt = new ProcessAtStationTask(step, _timeDelay);
            //    yield return processAt.Execute(_botContext, _timeDelay);
            //    Debug.Log("Done ProcessAt");
            //    break;
        }
    }



    // =============== Service ==============
    public void StartActionStep(List<BotStep> steps, KitchenType kitchenType)
    {
        StartCoroutine(ExecuteStep(steps, kitchenType));
    }    

}
