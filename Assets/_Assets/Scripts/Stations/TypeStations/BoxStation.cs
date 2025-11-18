using System.Collections;
using UnityEngine;

public class BoxStation : BaseStation, ISpawnerFoodRaw
{
    private static int _HAS_ANI_BOOL_ISOPEN = Animator.StringToHash("isOpen");

    [SerializeField] GameObject _foodPrefab;
    private bool _hasFood;  
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
        if(!_hasFood) _animator.SetBool(_HAS_ANI_BOOL_ISOPEN, false);
    }

    private IEnumerator WaitOneFrameToSpawnerAndPickUp(PickableObj food)
    {
        food.Init(_player, this);
        yield return null;
        food.PickUpObj(_player.GetTransformHoldFood());
    }    


    // ===================== ISpawnerFoodRaw ========================
    public PickableObj SpawnFood(PlayerInteraction interaction)
    {
        var objFood = PoolManager.Instance.Spawner(_foodPrefab, _transformHoldFood.position, Quaternion.identity, _transformHoldFood);
        if (objFood != null && objFood.TryGetComponent<PickableObj>(out PickableObj obj))
        {
            StartCoroutine(WaitOneFrameToSpawnerAndPickUp(obj));
            return obj;
        }    
        return null;
    }
}
