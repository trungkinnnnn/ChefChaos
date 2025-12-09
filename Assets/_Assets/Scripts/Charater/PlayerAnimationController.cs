using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private static int _HAS_ANI_BOOL_ISSERVICE = Animator.StringToHash("isService");
    private static int _HAS_ANI_BOOL_ISFIRE = Animator.StringToHash("isFire");
    private static int _HAS_ANI_BOOL_ISRUNNING = Animator.StringToHash("isRunning");
    private static int _HAS_ANI_BOOL_ISWASHINGDONE = Animator.StringToHash("isWasingDone");
    private static int _HAS_ANI_TRIGGER_ISWASHING = Animator.StringToHash("isWashing");
    private static int _HAS_ANI_TRIGGER_ISGRABAGE = Animator.StringToHash("isGrabage");
    private static int _HAS_ANI_TRIGGER_ISCUTTING = Animator.StringToHash("isCutting");

    private PlayerMovement _playerMovement;
    private Animator _animator;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateAniRunning();
    }

    private void UpdateAniRunning()
    {
        SetBoolAnimation(_HAS_ANI_BOOL_ISRUNNING, _playerMovement.IsMoving());
    }    

    private void SetBoolAnimation(int ani, bool value)
    {
        _animator.SetBool(ani, value);
    }

    private IEnumerator WaitTimeWasing(float time)
    {
        _animator.SetTrigger(_HAS_ANI_TRIGGER_ISWASHING);
        _animator.SetBool(_HAS_ANI_BOOL_ISWASHINGDONE, false);
        StartCoroutine(WaitTimeLockInput(time + 0.5f));
        yield return new WaitForSeconds(time - 0.5f);
        _animator.SetBool(_HAS_ANI_BOOL_ISWASHINGDONE, true);
    }


    private IEnumerator WaitTimeLockInput(float time)
    {
        _playerMovement.LockInput(true);
        yield return new WaitForSeconds(time);
        _playerMovement.LockInput(false);
    }


    // =============== Service ================
    public void SetBoolAnimationService(bool value)
    {
        _animator.SetBool(_HAS_ANI_BOOL_ISSERVICE, value);
    } 
        
    public void PlayAnimationCutting(float time)
    {
        _animator.SetTrigger(_HAS_ANI_TRIGGER_ISCUTTING);
        StartCoroutine(WaitTimeLockInput(time));
    }

    public void PlayAnimationWashing(float time)
    {
        StartCoroutine(WaitTimeWasing(time));
    }

    public void PlayAnimationGrabage(float time)
    {
        _animator.SetTrigger(_HAS_ANI_TRIGGER_ISGRABAGE);
        StartCoroutine(WaitTimeLockInput(time));
    }
   

}
