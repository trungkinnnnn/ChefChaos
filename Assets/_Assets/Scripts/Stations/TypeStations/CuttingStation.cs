using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingStation : BaseStation
{
    [SerializeField] GameObject _knife;
    private StationRecipeManager _stationRecipe;
    private float _timeWait = 0.2f;

    protected override void Start()
    {
        base.Start();
        _stationRecipe = GetComponent<StationRecipeManager>();
    }
    private bool CheckTypeFood(FoodData data)
    {
        return data.canCut;
    }

    protected override void PickableObj(PickableObj obj)
    {
        if(obj is FoodObj foodObj && CheckTypeFood(foodObj.GetDataFood()))
        {
            ProcessRule rule = _stationRecipe.GetProcessRule(foodObj.GetDataFood().foodType);
            if (rule == null) return;
            base.PickableObj(obj);
            SliceFood(foodObj, rule);
        }
    }

    private void SliceFood(FoodObj obj, ProcessRule rule)
    {
        
        StartCoroutine(WaitScecondSliceFood(obj, rule));
    }

    private IEnumerator WaitScecondSliceFood(FoodObj obj, ProcessRule rule)
    {
        yield return new WaitForSeconds(_timeWait);

        PoolManager.Instance.Despawner(obj.gameObject);
        ProgressBarManager.Instance.CreateProgressBar(_transformHoldFood, rule.processTime);

        _player.GetAnimationController().PlayAnimationCutting(rule.processTime + _timeWait);

        SpawnObjSlice(rule);
    }

    private void SpawnObjSlice(ProcessRule rule)
    {
        var objFood = PoolManager.Instance.Spawner(rule.resultPrefabs, _transformHoldFood.position, Quaternion.identity, _transformHoldFood);
        if (objFood != null && objFood.TryGetComponent<PickableObj>(out PickableObj pickObj))
        {
            pickObj.Init(null, this);
        }
    }


    // ============= Service =============
    public override void SetPickableObj(PickableObj obj)
    {
        _pickableObj = obj;
        _knife.SetActive(obj == null);
    }
}
