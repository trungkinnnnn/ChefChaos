
using UnityEngine;

public class FoodObj : PickableObj, ITrash
{
    private static int _HAS_ANI_TRIGGER_CUTTING = Animator.StringToHash("isCutting");

    [SerializeField] FoodData _foodData;
    private Animator _animator;

    protected override void OnEnable()
    {
        base.OnEnable();
        if(_animator == null) _animator = GetComponent<Animator>();
        if(_animator != null)_animator.SetTrigger(_HAS_ANI_TRIGGER_CUTTING);
    }

    public override void DoSomeThing()
    {
        base.DoSomeThing();
        var tryAddFood = _player.GetPickableObj().GetComponent<ITryAddFood>();
        if(tryAddFood == null ) return;
        tryAddFood.TryAddFood(this);
    }

    // =========== Interface (ITrash) =============
    public bool CanTrash() => true;
    public void TrashFood() => PoolManager.Instance.Despawner(gameObject);

    // =================== Service ===================
    public FoodData GetDataFood() => _foodData;

   
}


