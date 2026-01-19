using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/TaxData")]
public class TaxData : ScriptableObject
{
    public List<TaxLevel> taxLevels;
}

[System.Serializable]
public class TaxLevel
{
    public int id;
    public float value;
    public int price;
}
