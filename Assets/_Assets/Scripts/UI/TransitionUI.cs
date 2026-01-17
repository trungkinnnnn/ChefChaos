using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionUI : MonoBehaviour
{
    private string _colorDisable = "#CFCFCF";
    [Header("Color")]
    [SerializeField] List<string> _colorHex = new();
    [SerializeField] List<Image> _imageColorsButton = new(); 

    [Header("Button")]
    [SerializeField] List<Button> _buttonTransition = new();

    [Header("Screen")]
    [SerializeField] List<GameObject> _sceenUI = new();

    private int _currentIndex = 0;

    private void Start()
    {
        AddOnclickButton();
        SetUpColorStart(0);
        SetupScreenStart(0);
    }

    private void AddOnclickButton()
    {
        for(int i = 0; i < _buttonTransition.Count; i++)
        {
            int index = i;  
            _buttonTransition[i].onClick.AddListener(() => HandleChangeScreen(index));
        }    
    }

    private void SetUpColorStart(int index)
    {
        for (int i = 0; i < _sceenUI.Count; i++)
        {
            if(i == index)
            {
                _sceenUI[i].SetActive(true);
                continue;
            }
            _sceenUI[i].SetActive(false);
        }
    }

    private void SetupScreenStart(int index)
    {
        for (int i = 0; i < _imageColorsButton.Count; i++)
        {
            string colorHex = index == i ? _colorHex[index] : _colorDisable;
            if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
            {
                _imageColorsButton[i].color = color;
            }
        }
    }    

    private void HandleChangeScreen(int index)
    {
        ChangeColor(index);
        ChangeScreen(index);
        _currentIndex = index;
    }

    private void ChangeColor(int index)
    {
        OnDisableColor(_currentIndex);
        OnEnableColor(index);
    }   
    
    private void OnEnableColor(int index)
    {
        string colorHex = _colorHex[index];
        if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
        {
            _imageColorsButton[index].color = color;
        }
    }
    
    private void OnDisableColor(int index)
    {
        if (ColorUtility.TryParseHtmlString(_colorDisable, out Color color))
        {
            _imageColorsButton[index].color = color;
        }
    }    

    private void ChangeScreen(int index)
    {
        _sceenUI[_currentIndex].SetActive(false);
        _sceenUI[index].SetActive(true);
    }    

}
