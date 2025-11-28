using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour
{
    [SerializeField] List<Image> _imageShowFoodType;

    private Quaternion _initRotaion;


    private void Start()
    {
        _initRotaion = transform.rotation;
        ResetImages();
    }

    private void LateUpdate()
    {
        transform.rotation = _initRotaion;
    }

    public void AddSpriteFood(Sprite sprite)
    {
        foreach(var image  in _imageShowFoodType)
        {
            if(image.sprite == null)
            {
                image.sprite = sprite;
                image.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void ResetImages()
    {
        foreach(var image in _imageShowFoodType)
        {
            image.sprite = null;
            image.gameObject.SetActive(false);
        }    
    }    


}
