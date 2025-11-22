using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ISelectable
{
    void OnSelectable(PlayerInteraction player = null);
    void OnDeselectable();
    Transform GetSelectableTransform();
    void DoSomeThing();
}

public interface ICutting
{
    bool CanCutting();
}