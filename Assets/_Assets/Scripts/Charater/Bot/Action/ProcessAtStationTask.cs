using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class ProcessAtStationTask : ValidateTask
{
    private float _timeDelay;
    private StationType _stationType;
    private FoodType _outputFood;
    private float _waitTime;
    private IStation _stationTarget;

    public ProcessAtStationTask(StationType stationType, FoodType outputFood,float waitTime, float timeDelay = 0.2f)
    {
        _stationType = stationType;
        _outputFood = outputFood;
        _waitTime = waitTime;
        _timeDelay = timeDelay;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Not hold anything");
        var pick = context.Interaction.GetPickableObj();
        if (pick is not FoodObj) return TaskExecutionResult.Failed("Holding not food");

        _stationTarget = context.FindStationOne(_stationType);
        if (_stationTarget == null) return TaskExecutionResult.Failed("Cant find station");

        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        context.Movement.StartMoving(_stationTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
        yield return new WaitForSeconds(_timeDelay);

        if(_waitTime > 0)
        {
            yield return new WaitForSeconds(_waitTime);

            context.Movement.StartMoving(_stationTarget.GetSelectableTransform());
            yield return new WaitUntil(() => !context.Movement.IsMoving());
            yield return new WaitForSeconds(_timeDelay);
        }    

    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Faild to pick item");

        var pickup = context.Interaction.GetPickableObj();
        if (pickup is not FoodObj food || food.GetDataFood().foodType != _outputFood)
            return TaskExecutionResult.Retry("Wrong item");

        return TaskExecutionResult.Success();
    }
}
