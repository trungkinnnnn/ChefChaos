using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanPlateStackController : MonoBehaviour
{
    [SerializeField] PlateInfo _plateInfo;
    [SerializeField] Transform _cleanStackRoot;

    private BaseStation _baseStation;
    private List<PickableObj> _dirtyPlatePickables = new();
    private float _currentStackHeight = 0;

    private void Start()
    {
        _baseStation = GetComponent<BaseStation>();
    }

    private IEnumerator PlateCleanPlateStack(PickableObj obj, IPlateHidden plateHidden, float duration = 0.4f)
    {
        yield return new WaitForSeconds(duration);

        obj.transform.SetParent(_cleanStackRoot, false);
        obj.transform.localPosition = new Vector3(0, _currentStackHeight, 0);
        _currentStackHeight += plateHidden.GetTotalY();

        ClearDirtyPlatesFromPlateHidden(plateHidden.GetPickableObjs());
        plateHidden.ResetPlateHidden();
    }

    private void ClearDirtyPlatesFromPlateHidden(List<PickableObj> plateDirtyObjs)
    {
        if (plateDirtyObjs == null || plateDirtyObjs.Count == 0) return;
        foreach (var pickable in plateDirtyObjs)
        {
            if(pickable is not IPlate plate) continue;
            pickable.Init(null, _baseStation);
            pickable.transform.SetParent(_cleanStackRoot, true);
            pickable.gameObject.SetActive(false);
            plate.ResetPlate();
        }
        CacheDirtyPlate(plateDirtyObjs);
    }

    private void CacheDirtyPlate(List<PickableObj> plateDirtyObjs)
    {
        _dirtyPlatePickables.AddRange(plateDirtyObjs);
    }


    private float TakeHeightPlate(PickableObj obj)
    {
        KitchenType type = obj.GetDataPickableObj().typeObj;
        foreach(InfoData info in _plateInfo.Infos)
        {
            if(info.type == type) return info.size;
        }
        return 0;
    }    


    // ============= Service ==============
    public void AddCleanPlate(PickableObj obj, IPlateHidden plateHidden)
    {
        StartCoroutine(PlateCleanPlateStack(obj, plateHidden));
    }

    public void ActiveCleanPlate(List<PickableObj> plateDirtyObjs)
    {
        foreach (var plate in plateDirtyObjs)
        {
            plate.gameObject.SetActive(true);
            plate.ActiveCollider(true);
        }
    }

    public void HandleRemoveCleanPlate()
    {
        PickableObj obj = _dirtyPlatePickables[^1];
        float sizeHeigth = TakeHeightPlate(obj);
        _currentStackHeight -= sizeHeigth;  
        _dirtyPlatePickables.Remove(obj);
    }    

}
