using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKitchenTask : ValidateTask
{
    private StationType _stationDrop;
    private StationType _stationNear;

    private IStation _stationTarget;

    public DropKitchenTask(StationType stationDrop, StationType stationNear)
    {
        _stationDrop = stationDrop;
        _stationNear = stationNear;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        Debug.Log("Excute dropKitchen");
        _stationTarget = context.FindStationNear(_stationDrop, _stationNear);
        if (_stationTarget == null) return TaskExecutionResult.Failed($"No {_stationDrop} empty");

        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        context.Movement.StartMoving(_stationTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Still holding kitchen");
        if (_stationTarget.IsEmpty() && _stationTarget.GetTypeStation() != StationType.ServiceStation) 
            return TaskExecutionResult.Retry("Station not have kitchenTarget");
        return TaskExecutionResult.Success();
    }
}
