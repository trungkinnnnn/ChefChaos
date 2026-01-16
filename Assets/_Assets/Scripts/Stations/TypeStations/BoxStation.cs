using System.Collections;
using UnityEngine;

public class BoxStation : BaseStation
{
    private static int _HAS_ANI_BOOL_ISOPEN = Animator.StringToHash("isOpen");

    [SerializeField] FoodType _foodSpawn;        
    

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

    private IEnumerator WaitOneFrameToSpawnerAndPickUp(PickableObj food)
    {
        food.Init(_player, this);
        yield return null;
        food.PickUpObj(_player.GetTransformHoldFood());
    }

    public PickableObj SpawnFood(PlayerInteraction interaction)
    {
        GameObject foodSpawn = IngredientService.Instance.PurchaseIngredient(_foodSpawn, transform.position);
        if (foodSpawn == null) return null;
        var objFood = PoolManager.Instance.Spawner(foodSpawn, _transformHoldFood.position, Quaternion.identity, _transformHoldFood);
        if (objFood != null && objFood.TryGetComponent<PickableObj>(out PickableObj obj))
        {
            StartCoroutine(WaitOneFrameToSpawnerAndPickUp(obj));
            return obj;
        }
        return null;
    }

    // ===================== DO SomeThing ========================

    public override void DoSomeThing()
    {
        if (!_player.CheckNullPickUpObj()) return;
        PickableObj pickableObj = SpawnFood(_player);
        if (pickableObj == null) return;
        _player.SetPickUpObj(pickableObj);
    }

}
