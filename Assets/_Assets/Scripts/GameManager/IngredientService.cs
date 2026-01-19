using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IngredientService : MonoBehaviour
{
    public static IngredientService Instance;

    [Header("Data")]
    [SerializeField] IngredientDatabase _database;

    [Header("EconomySettin")]
    [SerializeField] Vector2 _tipBonusRanger = new Vector2(1, 5);

    private Dictionary<FoodType, InfoFood> _lookup = new();
    private PriceCalculator _priceCalculator;
    private OrderProcessor _orderProcessor;

    private float _ingredientDiscount = 0.1f;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        LoadFoodDatabase();
        InitializeServices();
    }

    private void InitializeServices()
    {
        _priceCalculator = new PriceCalculator(_ingredientDiscount);
        _orderProcessor = new OrderProcessor(_tipBonusRanger);
    }

    private void LoadFoodDatabase()
    {
        if (_database == null) return;
        foreach(var infoFood  in _database.foodList)
        {
            if(_lookup.ContainsKey(infoFood.foodType)) continue;
            _lookup.Add(infoFood.foodType, infoFood);
        }
    }

    private bool CanAfford(int price)
    {
        return MoneyService.Instance.HasEnoughPrice(price);
    }
    
    private void ProcessPurchase(int price, Vector3 position)
    {
        MoneyService.Instance.PlusSpentMoneyToday(price);
        MoneyService.Instance.MinusMoneyTotal(price);
        ShowPurchasingUI(position, price);
    }

    private void ShowPurchasingUI(Vector3 position, int price)
    {
        ContentTextUI.Instance.CreatePriceText(position, "- " + price.ToString());
    }    

    private void ShowOrderCompleteUI(Vector3 position, OrderResult orderResult)
    {
        ContentTextUI.Instance.CreatePriceText(
            position, 
            "+ " + orderResult.BasePrice.ToString(), 
            "+ " + orderResult.TipBonus.ToString()
        );
    }

    // =================== Service ====================

    public bool TryGetFoodInfo(FoodType foodType, out InfoFood foodInfo)
    {
        return _lookup.TryGetValue(foodType, out foodInfo);
    }

    public void CompletedOrder(Vector3 position, List<FoodType> orderedFoods)
    {
        if (orderedFoods.Count == 0 || orderedFoods == null)
        {
            Debug.Log("orderFoods non");
            return;
        }

        var orderResult = _orderProcessor.ProcessOrder(orderedFoods, _lookup);
        MoneyService.Instance.PlusEarnedMoneyToday(orderResult.BasePrice + orderResult.TipBonus);
        ShowOrderCompleteUI(position, orderResult);
    }

    public GameObject PurchaseIngredient(FoodType foodType, Vector3 positionSpawn)
    {
        if(!TryGetFoodInfo(foodType,out  InfoFood foodInfo)) return null;

        int finalPrice = _priceCalculator.CalculateDiscountPrice(foodInfo.basePrice);

        if(!CanAfford(finalPrice))
        {
            ContentTextUI.Instance.CreateInfoText(positionSpawn, TextInfo.NOT_HAVE_MONEY);
            return null;
        }

        ProcessPurchase(finalPrice, positionSpawn);
        return foodInfo.prefabFood;

    }

    public void UpdateDiscount(float value)
    {
        _ingredientDiscount = value;
        _priceCalculator.UpdateDiscountRate(value);
    }    

}
