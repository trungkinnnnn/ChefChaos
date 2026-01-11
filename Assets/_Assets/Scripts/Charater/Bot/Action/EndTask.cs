using System.Collections;

public class EndTask : ValidateTask
{
    private KitchenType _typekitchenTake;

    public void Init(KitchenType kitchenTake)
    {
        currentRetry = 0;
        _typekitchenTake = kitchenTake;
        if (kitchenTake != KitchenType.Plate) _typekitchenTake = KitchenType.PlateSoup;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Still Hoding something");
        return TaskExecutionResult.Success();
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        PickupKitchenTask pickupKitchenTask = new PickupKitchenTask();
        DropKitchenTask drop = new DropKitchenTask();
        pickupKitchenTask.Init(_typekitchenTake);
        drop.Init(StationType.ServiceStation, StationType.ServiceStation);

        yield return pickupKitchenTask.Execute(context);
        if (_typekitchenTake != KitchenType.Plate)
        {
            TakeSoupTask takeSoupTask = new TakeSoupTask();
            takeSoupTask.Init(_typekitchenTake);
            yield return takeSoupTask.Execute(context);
        }
        yield return drop.Execute(context);
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Drop kitchen to service faild");
        return TaskExecutionResult.Success();
    }
}
