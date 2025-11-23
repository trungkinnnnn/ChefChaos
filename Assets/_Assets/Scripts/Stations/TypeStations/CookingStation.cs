using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStation : BaseStation
{
    [SerializeField] GameObject _smokeCooking;
    [SerializeField] GameObject _fire;
    private ProgressBar _progressBarCurrent;
    private float _timeWait = 0.2f;
    private float _timeFireOn = 5f;
    private bool _isCooking = false;    
    private bool _isSpanwer = false;// Check obj spawner or pickup => Skip base.PickableObj    

    private bool CheckTypeFood(FoodData data)
    {
        return data.canCook;
    }

    protected override void PickableObj(PickableObj obj)
    {
        if (obj is FoodObj foodObj && CheckTypeFood(foodObj.GetDataFood()))
        {
            ProcessRule rule = RecipeManager.Instance.GetProcessRule(_stationType, foodObj.GetDataFood().foodType);
            if (rule == null) return;
            if(!_isSpanwer) base.PickableObj(obj);
            StartCooking(foodObj, rule); 
        }else
        {
            _isSpanwer = false;
            _smokeCooking.SetActive(false);
            if (_pickableObj != null) StartCoroutine(WaitTimeOnFire());
        }    
    }

    private void StartCooking(FoodObj obj, ProcessRule rule)
    {
        _isCooking = true;
        _smokeCooking.SetActive(_isCooking);
        StartCoroutine(WaitSecondeForCooking(obj, rule));   
    }


    private IEnumerator WaitSecondeForCooking(FoodObj foodObj, ProcessRule rule)
    {
        yield return new WaitForSeconds(_timeWait);
        _progressBarCurrent = ProgressBarManager.Instance.CreateProgressBar(_transformHoldFood, rule.processTime);

        float time = 0;
        while (time < rule.processTime)
        {
            time += Time.deltaTime;

            if(_isCooking == false)
            {
                _progressBarCurrent.DespawnerProgressBar();
                _smokeCooking.SetActive(_isCooking);
                yield break;
            }    

            yield return null;
        }

        PoolManager.Instance.Despawner(foodObj.gameObject);
        SpawnObjCooking(rule);
    }

    private void SpawnObjCooking(ProcessRule rule)
    {
        var objFood = PoolManager.Instance.Spawner(rule.resultPrefabs, _transformHoldFood.position, Quaternion.identity, _transformHoldFood);
        if (objFood != null && objFood.TryGetComponent<PickableObj>(out PickableObj pickObj))
        {
            pickObj.Init(null, this);
            _isSpanwer = true;
            PickableObj(pickObj);
        }
    }

    private IEnumerator WaitTimeOnFire()
    {
        float time = 0;
        while(time < _timeFireOn)
        {
            time += Time.deltaTime;

            if(_pickableObj == null) yield break;

            yield return null;
        }
        _fire.SetActive(true);
    }    

    // ============== Service =================
    public override void SetPickableObj(PickableObj obj)
    {
        _pickableObj = obj;
        if(obj == null)
        {
            _isCooking = false;
            _isSpanwer = false;
        }    
    }


}
