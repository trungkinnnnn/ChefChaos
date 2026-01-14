using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class StreetLight : MonoBehaviour
{
    private List<Light> _lights = new List<Light>();

    private void Start()
    {
        _lights.Clear();
        GetComponentLightAll();
        SetUpLights();
        EvenListening();
    }

    private void GetComponentLightAll()
    {
        var lights = GetComponentsInChildren<Light>();
        foreach (Light light in lights)
        {
            _lights.Add(light);
        }    
    }

    private void SetUpLights()
    {
        OffLight();
    }   
    
    private void EvenListening()
    {
        EventManager.StartListening(GameEventKeys.DayStarted, OffLight);
        EventManager.StartListening(GameEventKeys.NightStarted, OnLight);
    }   
    
    private void OnLight()
    {
        foreach (Light light in _lights) light.enabled = true;
    }   
    
    private void OffLight()
    {
        foreach (Light light in _lights) light.enabled = false;
    }    


}
