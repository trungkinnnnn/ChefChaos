using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowToTrashTask : ValidateTask
{
    private IStation _stationTarget;
    private float _timeWait = 2f;
    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Holding nothing");
        _stationTarget = context.FindStationOne(StationType.GarbageStation);
        if (_stationTarget == null) return TaskExecutionResult.Failed("Not found trashstation");

        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        context.Movement.StartMoving(_stationTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
        yield return new WaitForSeconds(_timeWait); 
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if(context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Success();
        var pickup = context.Interaction.GetPickableObj();
        if (pickup is IKitchen kitchen && kitchen.IsEmpty()) return TaskExecutionResult.Success();
        return TaskExecutionResult.Retry("Faild to throw to trash");
    }
}
