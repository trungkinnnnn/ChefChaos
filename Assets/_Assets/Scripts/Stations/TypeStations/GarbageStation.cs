using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageStation : BaseStation
{
    private static int _HAS_ANI_BOOL_ISOPEN = Animator.StringToHash("isOpen");
    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Animator>();
    }

    public override void OnSelectable(PlayerInteraction player = null)
    {
        base.OnSelectable(player);
        _animator.SetBool(_HAS_ANI_BOOL_ISOPEN, true);
    }

    public override void OnDeselectable()
    {
        base.OnDeselectable();
        _animator.SetBool(_HAS_ANI_BOOL_ISOPEN, false);
    }
}
