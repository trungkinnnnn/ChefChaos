using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotExecuteTaskOrder
{
    private float _timeDelay;
    private BotContext _botContext;
    private BotTaskCase _botTaskCase;
    public BotExecuteTaskOrder(BotContext botContext, float timeDelay = 0.7f)
    {
        _timeDelay = timeDelay;
        _botContext = botContext;
        _botTaskCase = new BotTaskCase();
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
                _botTaskCase.StartTask.Init(kitchenType);
                yield return _botTaskCase.StartTask.Execute(_botContext, _timeDelay);
                //Debug.Log("Done StartTask");
                break;
            case StepTask.EndTask:
                _botTaskCase.EndTask.Init(kitchenType);
                yield return _botTaskCase.EndTask.Execute(_botContext, _timeDelay);
                //Debug.Log("Done EndTask");
                break;
            case StepTask.PickUpFood:
                _botTaskCase.PickFood.Init(step);
                yield return _botTaskCase.PickFood.Execute(_botContext, _timeDelay);
                //Debug.Log("Done PickUpFood");
                break;
            case StepTask.DropFood:
                _botTaskCase.DropFood.Init(step);
                yield return _botTaskCase.DropFood.Execute(_botContext, _timeDelay);
                //Debug.Log("Done DropFood");
                break;
            case StepTask.ProcessAt:
                _botTaskCase.ProcessAt.Init(step);
                yield return _botTaskCase.ProcessAt.Execute(_botContext, _timeDelay);
                //Debug.Log("Done ProcessAt");
                break;

        }
    }

    // =============== Service ==============
    public IEnumerator StartActionStep(List<BotStep> steps, KitchenType kitchenType)
    {
        yield return ExecuteStep(steps, kitchenType);
    }

}
