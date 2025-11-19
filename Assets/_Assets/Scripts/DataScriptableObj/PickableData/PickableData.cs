using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PickableData")]
public class PickableData : ScriptableObject
{
    public string nameFood;
    public Sprite spriteFood;
    public Material materialDefault;
    public Material materialHighlight;
    public ObjType typeObj;
}
