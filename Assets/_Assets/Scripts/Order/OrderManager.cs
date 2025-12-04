using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] CookingRecipeDatabase _recipeDatabase;
    [SerializeField] GameObject _orderUIPrefab;
    [SerializeField] Transform _orderTransform;

    private OrderSpanwer _orderSpanwer;
    private float _timeWait = 0;
    private void Start()
    {
        _orderSpanwer = new OrderSpanwer(_recipeDatabase);
        //StartCoroutine(WaitSecondSpanwOrder());
    }

    private void Update()
    {
        if(Time.time < _timeWait) return;
        _timeWait = Time.time + 5f;
        StartCoroutine(WaitSecondSpanwOrder());
    }

    private IEnumerator WaitSecondSpanwOrder()
    {
        yield return new WaitForSeconds(3f);
        FoodRandom foodRandom = _orderSpanwer.RandomIngredientFood();
        OrderUI orderUI = CreateOrderUI();
        orderUI.Init(foodRandom);
    }    

    private OrderUI CreateOrderUI()
    {
        var obj = PoolManager.Instance.Spawner(_orderUIPrefab, _orderTransform.transform.position, Quaternion.identity, _orderTransform);
        if(obj.TryGetComponent<OrderUI>(out OrderUI orderUI)) return orderUI;
        return null;
    }    

}
