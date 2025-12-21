using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTask : ValidateTask
{
    private KitchenType _typekitchenTake;
    public EndTask(KitchenType kitchenTake)
    {
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
        PickupKitchenTask pickupKitchenTask = new PickupKitchenTask(_typekitchenTake);
        DropKitchenTask drop = new DropKitchenTask(StationType.ServiceStation, StationType.ServiceStation);

        yield return pickupKitchenTask.Execute(context);
        if(_typekitchenTake != KitchenType.Plate)
        {
            TakeSoupTask takeSoupTask = new TakeSoupTask(KitchenType.PlateSoup);
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
