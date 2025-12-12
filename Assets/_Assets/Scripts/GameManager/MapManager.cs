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

    private IEnumerator ShowInfoStation()
    {
        yield return new WaitForSeconds(2f);
        foreach(var key in _stations.Keys)
        {
            Debug.Log($"{key} + {_stations[key].Count}");
        }
    }

}
