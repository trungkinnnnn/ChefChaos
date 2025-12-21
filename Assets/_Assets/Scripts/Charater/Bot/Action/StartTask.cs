using System.Collections;
using UnityEngine;

public class StartTask
{
    private KitchenType _kitchenTarget;
    public StartTask (KitchenType kitchenTarget)
    {
        _kitchenTarget = kitchenTarget;
    }

    public IEnumerator Execute(BotContext context, float timeDelay = 0.5f)
    {
        PickupKitchenTask pickupKitchen = new PickupKitchenTask(_kitchenTarget);
        DropKitchenTask drop;
        if(_kitchenTarget != KitchenType.Plate)
        {
            drop = new DropKitchenTask(StationType.CookingSoupStation, StationType.CookingSoupStation);
        }else
        {
            drop = new DropKitchenTask(StationType.EmptyStation, StationType.SliceStation);
        }

        yield return pickupKitchen.Execute(context);
        yield return new WaitForSeconds(timeDelay);
        yield return drop.Execute(context);
    }
}
