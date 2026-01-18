using System.Collections;
using System.Collections.Generic;
using TigerForge;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyService : MonoBehaviour
{
    public static MoneyService Instance;

    private int _totalMoney = 25;
    private int _earnedMoneyToday;
    private int _spentMoneyToday;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        EventListening(); 
    }

    private void OnEnable()
    {
        ChangeTotalCoin();
    }


    private void EventListening()
    {
        EventManager.StartListening(GameEventKeys.DayStarted, ResetMoney);
    }

    private void ResetMoney()
    {
        _earnedMoneyToday = 0;
        _spentMoneyToday = 0;
    }

    private void ChangeTotalCoin()
    {
        EventManager.SetData(GameEventKeys.DataCointotal, _totalMoney);
        EventManager.EmitEvent(GameEventKeys.DataCointotal);
    }


    // ================ Service ================
    public void PlusEarnedMoneyToday(int money) => _earnedMoneyToday += money;
    public void PlusSpentMoneyToday(int money) => _spentMoneyToday += money;
    public void MinusMoneyTotal(int money)
    {
        _totalMoney -= money;
        ChangeTotalCoin();
    }    
    
    public void PlusTotalCoin(int coin)
    {
        _totalMoney += coin;
        ChangeTotalCoin();
    }  
    public bool HasEnoughPrice(int money) => money < _totalMoney;
    public int GetEarnedMoneyToday() => _earnedMoneyToday;  
    public int GetSpentMoneyToday() => _spentMoneyToday;

  
}
