using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotContext
{
    public BotMovement Movement { get; }
    public PlayerInteraction Interaction { get; }
    public IKitchen KitchenTarget { get; private set; }
    public BotContext (BotMovement movement, PlayerInteraction interaction)
    {
        Movement = movement;
        Interaction = interaction;
    }

    public IStation FindStationNear(StationType stationDrop, StationType stationNear)
    {
        Transform transformStationNear = FindStationOne(stationNear).GetSelectableTransform();
        List<IStation> stations = MapManager.Instance.GetTransformStationEmpty(stationDrop);
        if (stations == null || stations.Count == 0) return null;
        if(stations.Count == 1) return stations[0];

        IStation nearest = stations[0];    
        float minDist = Vector3.Distance(transformStationNear.position, nearest.GetSelectableTransform().position);
        foreach (var station in stations)
        {
            float dis = Vector3.Distance(transformStationNear.position, station.GetSelectableTransform().position);
            if(dis < minDist)
            {
                minDist = dis;
                nearest = station;
            }    
        }
        return nearest;
    }

    public IStation FindStationOne(StationType station)
    {
        var stations = MapManager.Instance.GetStationWithType(station);
        if (stations == null) return null;
        return stations[0];
    }

    public IKitchen FindKitchenEmpty(KitchenType type)
    {
        KitchenTarget = MapManager.Instance.GetKitchenEmpty(type);
        if (KitchenTarget == null) return null;
        return KitchenTarget;
    }

    public IKitchen FindKitchenOne(KitchenType type)
    {
        var kitchen = MapManager.Instance.GetKitchenOne(type);
        if (kitchen == null) return null;
        return kitchen;
    }

}
