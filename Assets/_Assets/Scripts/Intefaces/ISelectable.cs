using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public void OnSelectable(PlayerInteraction player = null);
    public void OnDeselectable();
    public Transform GetSelectableTransform();
}
