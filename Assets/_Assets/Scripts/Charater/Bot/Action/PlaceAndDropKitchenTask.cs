using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAndDropKitchenTask : ValidateTask
{
    private float _timeDelay;
    private KitchenType _kitchenType;
    private StationType _stationDrop;
    private StationType _stationNear;

    private IKitchen _kitchenTarget;
    private IStation _stationTarget;

    public PlaceAndDropKitchenTask(KitchenType kitchenType, StationType stationDrop, StationType stationNear, float timeDelay = 0.2f)
    {
        _kitchenType = kitchenType;
        _stationDrop = stationDrop;
        _stationNear = stationNear;
        _timeDelay = timeDelay;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        _kitchenTarget = context.FindKitchenEmpty(_kitchenType);
        if (_kitchenTarget == null) return TaskExecutionResult.Failed($"No {_kitchenType} empty");

        _stationTarget = context.FindStationNear(_stationDrop, _stationNear);
        if (_stationTarget == null) return TaskExecutionResult.Failed($"No {_stationDrop} empty");

        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        context.Movement.StartMoving(_kitchenTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
        yield return new WaitForSeconds(_timeDelay);

        context.Movement.StartMoving(_stationTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
        yield return new WaitForSeconds(_timeDelay);
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Still holding kitchen, failed to place");
        if(_stationTarget.IsEmpty()) return TaskExecutionResult.Retry("Station not have kitchenTarget");
        return TaskExecutionResult.Success();
    }

    protected override IEnumerator HandleValidationFailure(BotContext context, TaskExecutionResult result)
    {
        if(context.Interaction.CheckNullPickUpObj()) yield return null;
        var pickup = context.Interaction.GetPickableObj();
        if(pickup is IKitchen kitchen)
        {
            _stationTarget = context.FindStationNear(_stationDrop, _stationNear);
            context.Movement.StartMoving(_stationTarget.GetSelectableTransform());
            yield return new WaitUntil(() => !context.Movement.IsMoving());
            yield return new WaitForSeconds(_timeDelay);    
        }    
    }

}
