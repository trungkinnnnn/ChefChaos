using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningStation : BaseStation
{
    [SerializeField] GameObject _onWarter;

    private float _warhDuration = 2.5f;

    private static int _HAS_ANI_TRIGGER_ISCLOSEWARTER = Animator.StringToHash("isCloseWarter");
    private static int _HAS_ANI_TRIGGER_ISOPENWARTER = Animator.StringToHash("isOpenWarter");
    private Animator _ani;

    private CleanPlateStackController _cleanStack;

    protected override void Start()
    {
        base.Start();
        _ani = GetComponent<Animator>();
        _cleanStack = GetComponent<CleanPlateStackController>();
    }

    protected override void PickableObj(PickableObj obj)
    {
        if (obj is not IPlateHidden hidden) return;
        List<PickableObj> listPlateDirtyCopy = new List<PickableObj>(hidden.GetPickableObjs());
        _cleanStack.AddCleanPlate(obj, hidden);  
        base.PickableObj(obj);
        StartCoroutine(WaitTimeSecondsWasing(listPlateDirtyCopy));
    }

    private IEnumerator WaitTimeSecondsWasing(List<PickableObj> listPlateDirtyCopy)
    {
        float timeWait = listPlateDirtyCopy.Count * _warhDuration;
        _onWarter.SetActive(true);     
        PlayAnimationPlayerWashing(timeWait);
        ProgressBarManager.Instance.CreateProgressBar(_transformHoldFood, timeWait, 1.5f);
        
        _ani.SetTrigger(_HAS_ANI_TRIGGER_ISOPENWARTER);
        yield return new WaitForSeconds(timeWait);
        _ani.SetTrigger(_HAS_ANI_TRIGGER_ISCLOSEWARTER);

        _onWarter.SetActive(false);
        _cleanStack.ActiveCleanPlate(listPlateDirtyCopy);
        

    }

    private void PlayAnimationPlayerWashing(float time)
    {
        _player.GetAnimationController().PlayAnimationWashing(time);
    }    

    public override void SetPickableObj(PickableObj obj)
    {
        if(obj != null) return;
        _cleanStack.HandleRemoveCleanPlate();
        Debug.Log("remove plate");
    }

}
