using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] BotDataShop _botDataShop;

    [Header("Bot")]
    [SerializeField] List<GameObject> _botObjs;

    [Header("Button")]
    [SerializeField] List<Button> _buttonBuys;

    [Header("Text")]
    [SerializeField] List<TextMeshProUGUI> _textPrices;

    [Header("Check")]
    [SerializeField] List<GameObject> _completeds;

    private List<InfoBot> _bots;
    private string _botActive;

    private void Start()
    {
        SetupUI();
        LoadData();
    }

    private void SetupUI()
    {
        _bots = _botDataShop.Bots;
        FillActionButton();
        FillText();
        ActiveFalseCheck();
    }

    private void LoadData()
    {
        _botActive = SaveManager.LoadShopBot();
        TryParseInt(_botActive);
    }

    private void TryParseInt(string active)
    {
        if (string.IsNullOrEmpty(active)) return;
        var listActive = active.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                               .Select(int.Parse)
                               .ToList();
        for (int i = 0; i < listActive.Count; i++)
        {
            OpenBots(listActive[i]);
        }    
    }    
    private void FillActionButton()
    {
        for(int i = 0; i < _bots.Count; i++)
        {
            int id = _bots[i].id;
            _buttonBuys[i].onClick.AddListener(() => TryBuyBot(id));
        }
    }

    private void FillText()
    {
       
        for(int i = 0; i < _bots.Count; i++)
        {
            _textPrices[i].text = _bots[i].price.ToString();
        }
    }

    private void ActiveFalseCheck()
    {
        foreach(var completed in _completeds)
        {
            completed.SetActive(false);
        }
    }

    private void TryBuyBot(int id)
    {
        Debug.Log("id" + id);
        if (!CheckMoneyValid(_bots[id].price)) return;
        OpenBots(id);
        SaveData(id);
    }

    private void OpenBots(int id)
    {
        _botObjs[id].SetActive(true);
        _buttonBuys[id].gameObject.SetActive(false);
        _completeds[id].SetActive(true);
    }

    private void SaveData(int id)
    {
        MoneyService.Instance.MinusMoneyTotal(_bots[id].price);
        SaveManager.SaveTotalCoin(MoneyService.Instance.GetTotalCoin());

        _botActive += id + ",";
        SaveManager.SaveShopBot(_botActive);

        SaveManager.SaveData();
    }

    private bool CheckMoneyValid(int money)
    {
        return money <= MoneyService.Instance.GetTotalCoin();
    }


}
