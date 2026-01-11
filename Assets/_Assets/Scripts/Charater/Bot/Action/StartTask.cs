
using System.Collections;
using Unity.VisualScripting;

public class StartTask : ValidateTask
{
    private KitchenType _kitchenTarget;

    public void Init(KitchenType kitchenTarget)
    {
        _kitchenTarget = kitchenTarget;
        currentRetry = 0;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Still holding somthing");
        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        PickupKitchenTask pickupKitchen = new PickupKitchenTask();
        pickupKitchen.Init(_kitchenTarget);
        DropKitchenTask drop;
        if (_kitchenTarget != KitchenType.Plate)
        {
            drop = new DropKitchenTask();
            drop.Init(StationType.CookingSoupStation, StationType.CookingSoupStation);
        }
        else
        {
            drop = new DropKitchenTask();
            drop.Init(StationType.EmptyStation, StationType.SliceStation);
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
