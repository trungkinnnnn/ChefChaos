using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServiceStation : BaseStation
{
    [SerializeField] GameObject _plateHiddenPrefab;
    [SerializeField] Transform _positionSpawn;
    private float _timeSpawnPlate = 3f;
    private PlateEmptyHidde _plateHidden;
    protected override void Start()
    {
        base.Start();
        CreatPlateHidden();
    }

    private void CreatPlateHidden()
    {
        var obj = PoolManager.Instance.Spawner(_plateHiddenPrefab, _positionSpawn.position, Quaternion.identity, _positionSpawn);
        _plateHidden = obj?.GetComponent<PlateEmptyHidde>();
        _plateHidden.Init(null, this);
    }    

    protected override void PickableObj(PickableObj obj)
    {
        if(obj is not IPlate plate || obj is not IPlateContent plateContent) return;
        if(plate.GetStatePlate() == PlateState.Dirty) return;

        List<FoodType> foodTypes = plateContent.GetFoodTypes();

        if(foodTypes == null || foodTypes.Count == 0) return; 
        base.PickableObj(obj);

        StartCoroutine(DespawnPlateAndCreateWaitSeconds(obj));
        bool isMatch = TryOrderMatch(foodTypes);  
    }

    private bool TryOrderMatch(List<FoodType> food)
    {
        List<OrderUI> orders = OrderManager.Instance.GetOrders();
        if(orders == null) return false;
        foreach(OrderUI order in orders)
        {
            bool isMatch = CompareFoodAccuracy(order.GetFoods(), food);
            if(isMatch)
            {
                order.OrderCompleted();
                return true;
            }    
        }  
        return false;
    }    

    private bool CompareFoodAccuracy(List<FoodType> foodOders, List<FoodType> foodPlates)
    {
        var countA = foodOders.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());   
        var countB = foodPlates.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        foreach(var key in countA.Keys.Union(countB.Keys))
        {
            int aCount = countA.ContainsKey(key) ? countA[key] : 0;
            int bCount = countB.ContainsKey(key) ? countB[key] : 0;

           if(aCount != bCount) return false;
        }    
        return true;
    }    

    private IEnumerator DespawnPlateAndCreateWaitSeconds(PickableObj obj)
    {
        yield return new WaitForSeconds(1f);
        obj.gameObject.SetActive(false);
        if(obj is IPlate plate)
        {
            plate.ResetPlate();
            plate.SetDrityPlate();
        }    
        yield return new WaitForSeconds(_timeSpawnPlate);
        _plateHidden.TryAddPlate(obj);
        obj.gameObject.SetActive(true);
    }

    // ============= Service ===============

    public override void SetPickableObj(PickableObj obj)
    {
        if (obj != null) return;
        CreatPlateHidden();
    }
}
