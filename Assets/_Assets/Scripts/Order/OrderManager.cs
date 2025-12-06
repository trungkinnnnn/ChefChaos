using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [SerializeField] CookingRecipeDatabase _recipeDatabase;
    [SerializeField] GameObject _orderUIPrefab;
    [SerializeField] Transform _orderParent;
    [SerializeField] RectTransform _positionTop;
    [SerializeField] RectTransform _positionBottom;
    [SerializeField] RectTransform _positionSpawner;
    [SerializeField] float _spacingOderBox = 190;

    private List<OrderUI> _orders = new();
    private int _orderCount = 0;
    private int _orderMax = 3;

    private OrderSpanwer _orderSpanwer;
    private float _timeWait = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _orderSpanwer = new OrderSpanwer(_recipeDatabase);
    }

    private void Update()
    {
        if(Time.time < _timeWait || _orderCount == _orderMax) return;
        _timeWait = Time.time + 4f;
        StartCoroutine(WaitSecondSpanwOrder());
    }

    private IEnumerator WaitSecondSpanwOrder()
    {
        yield return new WaitForSeconds(5f);
        FoodRandom foodRandom = _orderSpanwer.RandomIngredientFood();
        var obj = CreateOrderUI(foodRandom);
        MoveInScreen(obj);
        yield return new WaitForSeconds(1.5f);
        RefreshPosition();
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

    // ============= Service ============
    public void RemoveOrderForList(OrderUI orderUI)
    {
        _orders.Remove(orderUI);
        _orderCount -= 1;
        RefreshPosition();
    }    

    public List<OrderUI> GetOrders()
    {
        if (_orders == null || _orders.Count <= 0) return null;
        return _orders;
    }

}
