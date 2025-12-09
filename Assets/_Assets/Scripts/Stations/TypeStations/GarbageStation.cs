using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageStation : BaseStation
{
    private float _timeGarbae = 0.8f;

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

    protected override void PickableObj(PickableObj obj)
    {
        if(obj is not ITrash trash) return;
        if (obj is FoodObj) _player.SetPickUpObj(null);
        if(trash.CanTrash()) StartCoroutine(WaitTimeTrash(trash));
    }

    private IEnumerator WaitTimeTrash(ITrash trash)
    {
        _player.GetAnimationController().PlayAnimationGrabage(_timeGarbae);
        yield return new WaitForSeconds(_timeGarbae);
        trash.TrashFood();
    }

}
