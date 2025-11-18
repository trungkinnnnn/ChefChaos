using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStation : BaseStation, ISpawnerFoodRaw
{
    private static int _HAS_ANI_BOOL_ISOPEN = Animator.StringToHash("isOpen");

    [SerializeField] GameObject _foodPrefab;
    private bool _hasFood;  
    private Animator _animator;

    private void Start()
    {
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

    private IEnumerator WaitOneFrameToSpawnerAndPickUp(FoodObj food)
    {
        food.Init(_player, this);
        yield return null;
        food.PickUpFood(_player.GetTransformHoldFood());
    }    


    // ===================== ISpawnerFoodRaw ========================
    public FoodObj SpawnFood(PlayerInteraction interaction)
    {
        FoodObj food;
        var objFood = PoolManager.Instance.Spawner(_foodPrefab, _transformHoldFood.position, Quaternion.identity, _transformHoldFood);
        if (objFood != null && objFood.TryGetComponent<FoodObj>(out FoodObj obj))
        {
            food = obj; 
            StartCoroutine(WaitOneFrameToSpawnerAndPickUp(obj));
            return food;
        }    
        return null;
    }
}
