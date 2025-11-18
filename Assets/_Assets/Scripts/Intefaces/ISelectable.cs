using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    void OnSelectable(PlayerInteraction player = null);
    void OnDeselectable();
    Transform GetSelectableTransform();
}
