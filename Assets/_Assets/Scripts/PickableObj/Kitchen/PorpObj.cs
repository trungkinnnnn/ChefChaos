using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorpObj : PickableObj
{
    protected override void ChangeStationValue(BaseStation station)
    {
        base.ChangeStationValue(station);
        gameObject.SetActive(station != null);
    }
}
