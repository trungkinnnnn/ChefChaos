using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningStation : BaseStation
{
    [SerializeField] Transform _positionMovePlate;
    [SerializeField] GameObject _onWarter;

    private float _warhDuration = 3f;
    private List<PickableObj> _pickables = new();

    private static int _HAS_ANI_TRIGGER_ISCLOSEWARTER = Animator.StringToHash("isCloseWarter");
    private static int _HAS_ANI_TRIGGER_ISOPENWARTER = Animator.StringToHash("isOpenWarter");
    private Animator _ani;

    protected override void Start()
    {
        base.Start();
        _ani = GetComponent<Animator>();    
    }

    protected override void PickableObj(PickableObj obj)
    {

        base.PickableObj(obj);
    }

}
