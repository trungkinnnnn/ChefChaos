using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStation : BaseStation, ISpawnerFoodRaw
{
    [SerializeField] GameObject _foodPrefab;
    [SerializeField] Transform _transformSpawner;

    private static int _HAS_ANI_BOOL_ISOPEN = Animator.StringToHash("isOpen");
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
        _animator.SetBool(_HAS_ANI_BOOL_ISOPEN, false);
    }


    // ===================== ISpawnerFoodRaw ========================
    public void SpawnFood(PlayerInteraction interaction)
    {
       PoolManager.Instance.Spawner(_foodPrefab, _transformSpawner.position, Quaternion.identity, _transformSpawner);
    }
}
