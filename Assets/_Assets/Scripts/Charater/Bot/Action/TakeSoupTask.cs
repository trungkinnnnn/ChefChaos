using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeSoupTask : ValidateTask
{
    private KitchenType _typeKitchenTake;
    private IKitchen _potTarget;

    public void Init(KitchenType kitchenType)
    {
        _typeKitchenTake = kitchenType;
        currentRetry = 0;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Hoding nothing");
        var pick = context.Interaction.GetPickableObj();
        if (pick is not IKitchen kitchen || kitchen.GetKitchenType() != _typeKitchenTake)
            return TaskExecutionResult.Failed("Not kitchen PlateSoup");

        _potTarget = context.FindKitchenOne(KitchenType.Pot);
        if (_potTarget == null) return TaskExecutionResult.Failed("Pot not found");

        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        context.Movement.StartMoving(_potTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        var kitchenHold = context.KitchenTarget;
        if (!kitchenHold.IsEmpty() && _potTarget.IsEmpty()) return TaskExecutionResult.Success();
        return TaskExecutionResult.Retry("Take soup faild");
    }

    protected override IEnumerator HandlePreconditionFailure(BotContext context, TaskExecutionResult result)
    {
        PickupKitchenTask pickKitchen = new PickupKitchenTask();
        DropKitchenTask drop = new DropKitchenTask();
        ThrowToTrashTask trashTask = new ThrowToTrashTask();

        pickKitchen.Init(_typeKitchenTake);
        drop.Init(StationType.EmptyStation, StationType.EmptyStation);
        

        if (context.Interaction.CheckNullPickUpObj())
        {
            yield return pickKitchen.Execute(context); yield break;
        }

        var pick = context.Interaction.GetPickableObj();
        if (pick is not IKitchen kitchen)
        {
            yield return trashTask.Execute(context);
            yield return pickKitchen.Execute(context);
            yield break;
        }

        if(kitchen.GetKitchenType() != KitchenType.PlateSoup)
        {
            yield return drop.Execute(context);
            yield return pickKitchen.Execute(context);
            yield break;
        }

    }

}
