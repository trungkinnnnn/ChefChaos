using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTaskCase
{
    public StartTask StartTask;
    public EndTask EndTask;
    public PickupFoodTask PickFood;
    public DropFoodTask DropFood;
    public ProcessAtStationTask ProcessAt;
    public PickUpKitchenDirty PickupKitchenDirty;
    public CleanKitchenDirty CleanKitchenDirty;
    public BotTaskCase()
    {
        StartTask = new StartTask();
        EndTask = new EndTask();
        PickFood = new PickupFoodTask();
        DropFood = new DropFoodTask();
        ProcessAt = new ProcessAtStationTask();
        PickupKitchenDirty = new PickUpKitchenDirty();
        CleanKitchenDirty = new CleanKitchenDirty();    
    }

}
