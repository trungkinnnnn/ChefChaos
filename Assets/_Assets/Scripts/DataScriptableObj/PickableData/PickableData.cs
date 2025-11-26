using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PickableData")]
public class PickableData : ScriptableObject
{
    public string nameObj;
    public Material materialDefault;
    public Material materialHighlight;
    public ObjType typeObj;
}
