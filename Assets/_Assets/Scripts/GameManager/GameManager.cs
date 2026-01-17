using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _fps = 60;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _fps;
    }

    private void Start()
    {
        EventListen();
    }

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.DataEndDayReady, StopTimeScale);
    }

    private void StopTimeScale() => Time.timeScale = 0;    
 
}
