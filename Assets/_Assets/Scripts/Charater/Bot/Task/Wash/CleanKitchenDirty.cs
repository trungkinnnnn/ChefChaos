using System.Collections;
using UnityEngine;

public class CleanKitchenDirty : ValidateTask
{
    private PickableObj _kitchenDirty;
    private float _timeWarhDuration = 2.5f;

    public CleanKitchenDirty(PickableObj kitchen)
    {
        _kitchenDirty = kitchen;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        Debug.Log("Cleaning");
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Bot not holding");
        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        float totalTime = 0;    
        if(_kitchenDirty is not IPlateHidden listKitchenDrity) yield break;
        totalTime = listKitchenDrity.GetPickableObjs().Count * _timeWarhDuration;

        IStation stationTarget = context.FindStationOne(StationType.CleanStation);
        if (stationTarget == null) yield break;

        context.Movement.StartMoving(stationTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
        yield return new WaitForSeconds(totalTime);
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Success();
        return TaskExecutionResult.Retry("Try drop one more");
    }



}