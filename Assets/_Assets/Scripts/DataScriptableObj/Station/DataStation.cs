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
    public StationType stationType;
    public List<KitchenValid> kitchenValids;
}
    
[Serializable]
public class KitchenValid
{
    public KitchenType type;
}


public enum KitchenType
{
    All,
    Pot,
    Plate,
    PlateSoup,
    Food,
    Porp,
}