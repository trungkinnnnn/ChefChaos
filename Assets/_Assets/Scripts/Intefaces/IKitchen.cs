using UnityEngine;

public interface IKitchen
{
    bool IsEmpty();
    PickableObj GetPickableObj();
    KitchenType GetKitchenType();
    Transform GetSelectableTransform();

}
