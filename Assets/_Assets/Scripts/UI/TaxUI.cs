using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaxUI : MonoBehaviour
{
    [SerializeField] TaxData _data;

    [Header("Button")]
    [SerializeField] Button _buttonBuy;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textCoin;
    [SerializeField] TextMeshProUGUI _beforeText;
    [SerializeField] TextMeshProUGUI _afterText;
    

    [Header("Check")]
    [SerializeField] GameObject _checkObj;

    List<TaxLevel> _taxLevelList;
    private int _currentLevel = 0;

    private void Start()
    {
        _buttonBuy.onClick.AddListener(TryBuyTax);
        LoadData();
        SetupUI();
        UpdateTaxForCalculateEndDay();
    }

    private void LoadData()
    {
        _currentLevel = SaveManager.LoadTaxLevel();
        _taxLevelList = _data.taxLevels;
    }

    private void SetupUI()
    {
        if(CheckLevelMax(_currentLevel))
        {
            UILevelMax();
            return;
        }
        FillText();
    }
 
    private void FillText()
    {
        _textCoin.text = _taxLevelList[_currentLevel + 1].price.ToString();
        _beforeText.text = (int)(_taxLevelList[_currentLevel].value * 100) + "%";
        _afterText.text = (int)(_taxLevelList[_currentLevel + 1].value * 100) + "%";
        _checkObj.gameObject.SetActive(false);
    }    

    private bool CheckLevelMax(int level)
    {
        if (_currentLevel != _taxLevelList[^1].id) return false;
        return true;
    }

    private void UILevelMax()
    {
        _beforeText.text = (int)(_taxLevelList[^1].value * 100) + "%";
        _afterText.text = "MAX";
        _buttonBuy.gameObject.SetActive(false);
        _checkObj.SetActive(true);
    }    

    private void TryBuyTax()
    {
        int price = _taxLevelList[_currentLevel + 1].price;
        if (!CheckMoneyValid(price)) return; 
        _currentLevel = _taxLevelList[_currentLevel + 1].id;
        MoneyService.Instance.MinusMoneyTotal(price);
        SaveData();
        SetupUI();
        UpdateTaxForCalculateEndDay();
    } 

    private void SaveData()
    {
        SaveManager.SaveTaxLevel(_currentLevel);
        SaveManager.SaveTotalCoin(MoneyService.Instance.GetTotalCoin());
        SaveManager.SaveData();
    }    

    private bool CheckMoneyValid(int price)
    {
        return price <= MoneyService.Instance.GetTotalCoin();
    }

    private void UpdateTaxForCalculateEndDay()
    {
        EndDayManager.Instance.UpdateTaxPer(_taxLevelList[_currentLevel].value);
    }


}
