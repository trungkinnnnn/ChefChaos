using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class ProgessUI : MonoBehaviour
{
    [SerializeField] ProgessData _data;
    [SerializeField] List<GameObject> _unlockObj;

    private List<DayLevel> _days;
    private int _currentDaylevel = 0;

    private void Start()
    {
        EventListen();
        LoadData();
        SetupUI();
        UpdateDataOrder();
    } 

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.DayStarted, UpDayLevel);
    }

    private void LoadData()
    {
        _currentDaylevel = SaveManager.LoadDayLevel();
        _days = _data.daylevels;
    }

    private void SetupUI()
    {
        for(int i = 0; i < _days.Count; i++)
        {
            if(_currentDaylevel >= _days[i].id)
            {
                _unlockObj[i].SetActive(true);
            }else
            {
                break;
            }
        }
    }

    private void UpDayLevel()
    {
        if(!CheckDayLevel()) return;
        _currentDaylevel = _days[_currentDaylevel + 1].id;
        SetupUI();
        UpdateDataOrder();
        SaveData();
    }

    private void UpdateDataOrder()
    {
        OrderManager.Instance.UpdateDataOrder(_data.daylevels[_currentDaylevel].database);
    }

    private void SaveData()
    {
        SaveManager.SaveDayLevel(_currentDaylevel);
        SaveManager.SaveData();

    }

    private bool CheckDayLevel()
    {
        int currentDay = DayNightCycle.Instance.GetDay();
        if (_currentDaylevel + 1 >= _days.Count) return false;
        if(currentDay < _days[_currentDaylevel + 1].dayLevel) return false;
        return true;
    }
            

}
