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

    private void Start()
    {
        AddOnclickButton();
        ChangeColor(0);
    }

    private void AddOnclickButton()
    {
        for(int i = 0; i < _buttonTransition.Count; i++)
        {
            int index = i;  
            _buttonTransition[i].onClick.AddListener(() => HandleChangeScreen(index));
        }    
    }

    private void HandleChangeScreen(int index)
    {
        ChangeColor(index);
    }

    private void ChangeColor(int index)
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

}
