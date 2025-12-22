using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupKitchenTask : ValidateTask
{
    private KitchenType _kitchenType;

    private IKitchen _kitchenTarget;

    public PickupKitchenTask(KitchenType kitchenType)
    {
        _kitchenType = kitchenType;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Holding somthing");

        _kitchenTarget = context.KitchenTarget;
        if(_kitchenTarget != null && _kitchenTarget.GetKitchenType() == _kitchenType) return TaskExecutionResult.Success("Check precoditions true");
      
        _kitchenTarget = context.FindKitchenEmpty(_kitchenType);
        if (_kitchenTarget == null) return TaskExecutionResult.Failed($"No {_kitchenType} empty");

        return TaskExecutionResult.Success("Check precoditions true");
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        context.Movement.StartMoving(_kitchenTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry($"Faild to pick kitchen {_kitchenType}");
        var pick = context.Interaction.GetPickableObj();
        if(pick is IKitchen kitchen && kitchen.GetKitchenType() == _kitchenType) return TaskExecutionResult.Success();
        return TaskExecutionResult.Retry("Wrong pickup kitchen");
    }

    protected override IEnumerator HandleValidationFailure(BotContext context, TaskExecutionResult result)
    {
        if (context.Interaction.CheckNullPickUpObj()) yield return null;
        var pick = context.Interaction.GetPickableObj();

        if(pick is IKitchen kitchen)
        {
            DropKitchenTask drop = new DropKitchenTask(StationType.EmptyStation, StationType.EmptyStation);
            yield return drop.Execute(context);
        }
        else
        {
            ThrowToTrashTask trashTask = new ThrowToTrashTask();
            yield return trashTask.Execute(context);
        }
        
    }

}
