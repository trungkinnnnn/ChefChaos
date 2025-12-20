using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public Dictionary<StationType, List<IStation>> _stations = new();
    public Dictionary<KitchenType, List<IKitchen>> _kitchens = new();

    private void Awake()
    {
        Instance = this;
        //StartCoroutine(ShowInfoStation());
    }

    // ================ Service ============== 

    public void AddIStation(IStation station)
    {
        StationType type = station.GetTypeStation();
        if(!_stations.ContainsKey(type))
        {
            _stations[type] = new List<IStation>();
        }    
        _stations[type].Add(station);
    }  
    
    public void AddIKitchen(IKitchen kitchen)
    {
        KitchenType type = kitchen.GetKitchenType();
        if(!_kitchens.ContainsKey(type))
        {
            _kitchens[type] = new List<IKitchen>();
        }
        _kitchens[type].Add(kitchen);
    }    



    // ================ Service ================
    public IKitchen GetKitchenEmpty(KitchenType type)
    {
        if (!_kitchens.ContainsKey(type)) return null; 
        var kitchens = _kitchens[type];

        foreach(var kitchen in kitchens)
        {
            if (kitchen.IsEmpty()) return kitchen;
        } 
        return null;
    }

    public List<IStation> GetTransformStationEmpty(StationType type)
    {
        if (!_stations.ContainsKey(type)) return null;
        var stations = _stations[type];

        List<IStation> stationValids = new List<IStation>();

        foreach (var station in stations)
        {
            if (station.IsEmpty()) stationValids.Add(station);
        }
        return stationValids;
    }

    public List<IStation> GetStationWithType(StationType type)
    {
        if (!_stations.ContainsKey(type)) return null;
        return _stations[type];
    }  

}
