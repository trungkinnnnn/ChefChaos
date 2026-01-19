
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class StationFlow : MonoBehaviour
{
    [SerializeField] ProgessData _data;
    [SerializeField] List<GameObject> _objStations;

    private List<DayLevel> _dayLevels;
    private void Start()
    {
        _dayLevels = _data.daylevels;
        EventListen();
    }

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.DayStarted, ActiveStation);
    }    
    
    private void ActiveStation()
    {
        int currentDay = DayNightCycle.Instance.GetDay();
        for(int i = 0; i< _dayLevels.Count; i++)
        {
            if(currentDay >= _dayLevels[i].dayLevel)
            {
                _objStations[i].SetActive(true);
            }
            else
            {
                break;
            }
        }
        
    }

}
