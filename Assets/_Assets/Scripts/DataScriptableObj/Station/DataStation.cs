using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DataStation")]
public class DataStation : ScriptableObject
{
    public string nameStation;
    public Material materialDefault;
    public Material materialHighlight;
}
