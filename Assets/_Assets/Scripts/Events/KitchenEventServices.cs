using TigerForge;
using UnityEngine;

public static class KitchenEventServices
{
    public static void EmitKitchenDirty(GameObject kitchen)
    {
        EventManager.SetData(GameEventKeys.KitchenDirty, kitchen);
        EventManager.EmitEvent(GameEventKeys.KitchenDirty, kitchen);
    }
}
