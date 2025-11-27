using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DataStation")]
public class DataStation : ScriptableObject
{
    public string nameStation;
    public Material materialDefault;
    public Material materialHighlight;
    public List<PlaceType> placeTypes;
}
    
[Serializable]
public class PlaceType
{
    public ObjType type;
}


public enum ObjType
{
    All,
    Pot,
    Plate,
    PlateSoup,
    Food,
    Porp,
}