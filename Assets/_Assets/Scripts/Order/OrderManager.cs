using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    // Animation Order
    [SerializeField] CookingRecipeDatabase _recipeDatabase;
    [SerializeField] GameObject _orderUIPrefab;
    [SerializeField] Transform _orderParent;
    [SerializeField] RectTransform _positionTop;
    [SerializeField] RectTransform _positionBottom;
    [SerializeField] RectTransform _positionSpawner;
    [SerializeField] float _spacingOderBox = 190;

    // Order Setting
    private List<OrderUI> _orders = new();
    private int _orderCount = 0;
    private int _orderMax = 3;
    private float _timeSpawnOrder = 4f;

    // Count Order
    private int _totalOrderSpawnerInday = 0;
    private int _totalOrderCompeletedInDay = 0; 
    private bool _isNight = false;

    // Service Order
    private OrderSpanwer _orderSpanwer;
    private Coroutine _orderSpawnCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        _orderSpanwer = new OrderSpanwer(_recipeDatabase);
        EventListening();
    }

    private void EventListening()
    {
        EventManager.StartListening(GameEventKeys.DayStarted, OnDay);
        EventManager.StartListening(GameEventKeys.NightStarted, OnNight);
    }    

    private void OnDay()
    {   
        StartNewDay();
        _orderSpawnCoroutine = StartCoroutine(OrderSpawnLoop());
    }    

    private void OnNight()
    {
        if(_orderSpawnCoroutine != null) StopCoroutine(_orderSpawnCoroutine);
        _isNight = true;
        TryEndDay();
    }    


    private IEnumerator OrderSpawnLoop()
    {
        yield return new WaitForSecondsRealtime(1f);
        while (true)
        {
            if(_orderCount < _orderMax)
            {
                FoodRandom foodRandom = _orderSpanwer.RandomIngredientFood();
                var obj = CreateOrderUI(foodRandom);

                MoveInScreen(obj);
                yield return new WaitForSeconds(1.5f);
                RefreshPosition();
                _totalOrderSpawnerInday++;
            }    
            yield return new WaitForSeconds(_timeSpawnOrder);
        }    
    }    

    private GameObject CreateOrderUI(FoodRandom foodRandom)
    {
        var obj = PoolManager.Instance.Spawner(_orderUIPrefab,Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(_orderParent, false);
        obj.GetComponent<RectTransform>().anchoredPosition = _positionSpawner.anchoredPosition;
        if(obj.TryGetComponent<OrderUI>(out OrderUI orderUI))
        {
            orderUI.Init(foodRandom, this);
            _orders.Add(orderUI);
            _orderCount += 1;
        }
        return obj;
    }    

    private void MoveInScreen(GameObject obj, float duration = 1f)
    {
        obj.GetComponent<RectTransform>().DOAnchorPos(_positionBottom.anchoredPosition, duration).SetEase(Ease.OutQuad);
    }


    private void RefreshPosition(float duration = 1.5f)
    {
        if(_orders.Count == 0) return;

        for(int i = 0; i < _orders.Count; i+= 1)
        {
            float targetY = -i * _spacingOderBox + _positionTop.anchoredPosition.y;
            RectTransform transform = _orders[i].GetComponent<RectTransform>();
            transform.DOAnchorPos(new Vector3(0, targetY, 0), duration).SetEase(Ease.OutQuad);
        }    

    }    

    private void TryEndDay()
    {
        if(!_isNight || _orderCount != 0) return;
        EventManager.EmitEvent(GameEventKeys.DayEnded);
    }


    private void StartNewDay()
    {
        _totalOrderCompeletedInDay = 0;
        _totalOrderSpawnerInday = 0;
        _isNight = false;
    }

    // ================== Service =================

    public void PlusOrderCompletedInDay() => _totalOrderCompeletedInDay += 1;

    public int GetTotalOrderCompletedInday() => _totalOrderCompeletedInDay;
    public int GetTotalOrderSpawnerInday() => _totalOrderSpawnerInday;

    public void RemoveOrderForList(OrderUI orderUI)
    {
        _orders.Remove(orderUI);
        _orderCount -= 1;
        RefreshPosition();

        TryEndDay();
    }    

    public List<OrderUI> GetOrders()
    {
        if (_orders == null || _orders.Count <= 0) return null;
        return _orders;
    }

    // Testing for bot task planner
    public OrderUI GetOrderNotSelected()
    {
        foreach(OrderUI orderUI in _orders)
        {
            if(!orderUI.IsSelected()) return orderUI;
        }
        return null;
    }
}
