
using UnityEngine;

public class FoodObj : PickableObj
{
    private static int _HAS_ANI_TRIGGER_CUTTING = Animator.StringToHash("isCutting");

    [SerializeField] FoodData _foodData;
    private Animator _animator;

    private void OnEnable()
    {
        if(_animator == null) _animator = GetComponent<Animator>();
        if(_animator != null)_animator.SetTrigger(_HAS_ANI_TRIGGER_CUTTING);
    }


    // =================== Service ===================
    public FoodData GetDataFood() => _foodData; 


}


