using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscountUI : MonoBehaviour
{
    [SerializeField] DiscountData _data;

    [Header("Button")]
    [SerializeField] Button _buttonBuy;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _titalText;
    [SerializeField] TextMeshProUGUI _afterText;
    [SerializeField] TextMeshProUGUI _beforeText;

    [Header("Check")]
    [SerializeField] GameObject _checkObj;

    List<DiscountLevel> _discountLevelList;
    private int _currentLevel = 0;
    private int _exampleTomato = 10;

    private void Start()
    {
        _buttonBuy.onClick.AddListener(TryBuyDiscount);
        LoadData();
        SetupUI();
        UpdateDiscountForIngredient();
    }

    private void LoadData()
    {
        _currentLevel = SaveManager.LoadDiscountLevel();
        _discountLevelList = _data.discountLevels;
    }

    private void SetupUI()
    {
        if (CheckLevelMax(_currentLevel))
        {
            UILevelMax();
            return;
        }
        FillText();
    }

    private void FillText()
    {
        _titalText.text ="-" + (int)(_discountLevelList[_currentLevel].value * 100) + "%";
        _beforeText.text = _exampleTomato.ToString();
        _afterText.text = CalculateExample(_discountLevelList[_currentLevel + 1].value).ToString();
        _checkObj.gameObject.SetActive(false);
    }

    private bool CheckLevelMax(int level)
    {
        if (_currentLevel != _discountLevelList[^1].id) return false;
        return true;
    }

    private void UILevelMax()
    {
        _titalText.text = "-" + (int)(_discountLevelList[_currentLevel].value * 100) + "%";
        _beforeText.text = _exampleTomato.ToString();
        _afterText.text = CalculateExample(_discountLevelList[^1].value).ToString();
        _buttonBuy.gameObject.SetActive(false);
        _checkObj.SetActive(true);
    }

    private void TryBuyDiscount()
    {
        int price = _discountLevelList[_currentLevel + 1].price;
        if (!CheckMoneyValid(price)) return;
        _currentLevel = _discountLevelList[_currentLevel + 1].id;
        MoneyService.Instance.MinusMoneyTotal(price);
        SaveData();
        SetupUI();
        UpdateDiscountForIngredient();
    }

    private void SaveData()
    {
        SaveManager.SaveDiscountLevel(_currentLevel);
        SaveManager.SaveTotalCoin(MoneyService.Instance.GetTotalCoin());
        SaveManager.SaveData();
    }

    private bool CheckMoneyValid(int price)
    {
        return price <= MoneyService.Instance.GetTotalCoin();
    }

    private void UpdateDiscountForIngredient()
    {
        IngredientService.Instance.UpdateDiscount(_discountLevelList[_currentLevel].value);
    }

    private int CalculateExample(float value)
    {
        return (int)(_exampleTomato - _exampleTomato * value);
    }    

}
