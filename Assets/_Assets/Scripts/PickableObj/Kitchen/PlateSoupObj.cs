using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSoupObj : PickableObj
{
    private List<FoodType> _foodTypes = new();
    private SoupContentVisual _soupVisual;

    protected override void Start()
    {
        base.Start();
        _soupVisual = GetComponent<SoupContentVisual>();
    }

    // =================== Service ===================
    public override bool HandheldReceiveCooked(List<FoodType> foodTypes, ObjType type)
    {
        if(_pickableData.typeObj != type) return false;
        _foodTypes = foodTypes;
        _soupVisual.OnVisual(foodTypes[0]);
        return true;
    }

}
