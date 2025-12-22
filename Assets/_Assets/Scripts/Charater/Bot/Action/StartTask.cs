
using System.Collections;

public class StartTask : ValidateTask
{
    private KitchenType _kitchenTarget;
    public StartTask(KitchenType kitchenTarget)
    {
        _kitchenTarget = kitchenTarget;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Still holding somthing");
        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        PickupKitchenTask pickupKitchen = new PickupKitchenTask(_kitchenTarget);
        DropKitchenTask drop;
        if (_kitchenTarget != KitchenType.Plate)
        {
            drop = new DropKitchenTask(StationType.CookingSoupStation, StationType.CookingSoupStation);
        }
        else
        {
            drop = new DropKitchenTask(StationType.EmptyStation, StationType.SliceStation);
        }

        yield return pickupKitchen.Execute(context);
        yield return drop.Execute(context);
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Success();
        return TaskExecutionResult.Retry("Faild drop kitchen");
    }
}
