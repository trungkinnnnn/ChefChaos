using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSelected : MonoBehaviour
{
    private static int _HAS_ANI_TRIGGER_ISSELECTED = Animator.StringToHash("isSelected");

    [SerializeField] Image _imageSelected;
    [SerializeField] Button _buttonSelected;

    private Animator _ani;
    private Sprite _currentSprite;
    
    private bool _isSelected = false;

    private void Start()
    {
        _ani = GetComponentInChildren<Animator>();
        _buttonSelected.onClick.AddListener(OnSelectedByPlayer);
        _currentSprite = _imageSelected.sprite;
        _imageSelected.enabled = false;
    }

    private void OnDisable()
    {
        _isSelected = false;
        _buttonSelected.enabled = true;
        _imageSelected.enabled = false;
    }

    private void OnSelectedByPlayer()
    {
        _isSelected = !_isSelected;
        _ani.SetBool(_HAS_ANI_TRIGGER_ISSELECTED, _isSelected);
        SetImage(_currentSprite);
        _imageSelected.enabled = true;
    }
    
    private void SetImage(Sprite sprite)
    {
        _imageSelected.sprite = sprite;
        _imageSelected.SetNativeSize();
    }

    // ============== Service ==============

    public bool IsSelected() => _isSelected;

    public void OnSelectedByBot(Sprite spriteBot)
    {
        _buttonSelected.enabled = false;
        _ani.SetBool(_HAS_ANI_TRIGGER_ISSELECTED, true);
        SetImage(spriteBot);
        _imageSelected.enabled = true;
    }
}
