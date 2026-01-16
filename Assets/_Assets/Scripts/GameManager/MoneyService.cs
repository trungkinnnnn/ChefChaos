using System.Collections;
using System.Collections.Generic;
using TigerForge;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyService : MonoBehaviour
{
    public static MoneyService Instance;

    private int _totalMoney = 20;
    private int _earnedMoneyToday;
    private int _spentMoneyToday;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        EventListening();
    }

    private void EventListening()
    {
        EventManager.StartListening(GameEventKeys.NightStarted, ShowMoney);
    }

    private void ShowMoney()
    {
        ShowEarnedMoneyToday();
        ShowSpentMoneyToday();
    }

    private void ShowEarnedMoneyToday()
    {
        Debug.Log("EarnedMoney today : " +  _earnedMoneyToday);
        _earnedMoneyToday = 0;
    }

    private void ShowSpentMoneyToday()
    {
        Debug.Log("SpentMoney today : " + _spentMoneyToday);
        _spentMoneyToday = 0;
    }

    // ================ Service ================
    public void PlusEarnedMoneyToday(int money)
    {
        _earnedMoneyToday += money;
    }

    public void PlusSpentMoneyToday(int money)
    {
        _spentMoneyToday += money;
    }

    public void MinusMoneyTotal(int money)
    {
        _totalMoney -= money;
    }    

    public bool HasEnoughPrice(int money)
    {
        return money < _totalMoney;
    }

}
