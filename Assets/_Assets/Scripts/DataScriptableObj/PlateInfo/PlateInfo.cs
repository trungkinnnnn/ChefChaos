
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/InfoPlate")]
public class PlateInfo : ScriptableObject
{
    public List<InfoData> Infos;
}

[System.Serializable]
public class InfoData
{
    public ObjType type;
    public float size;
}
