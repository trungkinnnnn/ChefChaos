using UnityEngine;

public interface IPickable
{
    void PickUp(PlayerInteraction player, Transform transform, BaseStation baseStation = null);
    BaseStation GetSelectableStation();
}