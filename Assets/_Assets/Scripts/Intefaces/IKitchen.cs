using UnityEngine;

public interface IKitchen
{
    PickableObj GetPickableObj();
    KitchenType GetKitchenType();
    Transform GetSelectableTransform();
}