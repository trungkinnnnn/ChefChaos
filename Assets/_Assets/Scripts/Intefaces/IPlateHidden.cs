using System.Collections.Generic;

public interface IPlateHidden
{
    public List<PickableObj> GetPickableObjs();
    public float GetTotalY();
    public void ResetPlateHidden();
}