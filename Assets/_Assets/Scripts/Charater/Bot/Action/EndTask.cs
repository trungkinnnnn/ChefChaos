using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTask : IBotTask
{
    private KitchenType _typekitchenTake;
    public EndTask(KitchenType kitchenTake)
    {
        _typekitchenTake = kitchenTake;
        if (kitchenTake != KitchenType.Plate) _typekitchenTake = KitchenType.PlateSoup;
    }
    public IEnumerator Execute(BotContext context, float timeDelay = 0.5f)
    {
        PickupKitchenTask pickupKitchenTask = new PickupKitchenTask(_typekitchenTake);
        DropKitchenTask drop = new DropKitchenTask(StationType.ServiceStation, StationType.ServiceStation);

        yield return pickupKitchenTask.Execute(context);
        yield return new WaitForSeconds(timeDelay);
        if(_typekitchenTake != KitchenType.Plate)
        {
            TakeSoupTask takeSoupTask = new TakeSoupTask(KitchenType.PlateSoup);
            yield return takeSoupTask.Execute(context);
            yield return new WaitForSeconds(timeDelay);
        }
        yield return drop.Execute(context);
    }
}
