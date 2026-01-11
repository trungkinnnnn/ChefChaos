using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OrderUI : MonoBehaviour
{
    [SerializeField] TypeCook _dataTypeCook;
    [SerializeField] TypeCook _listFoodNotShow;
    [SerializeField] GameObject _ingerdientsObj; 
    [SerializeField] GameObject _FoodCompletedObj; 
    [SerializeField] Image _imageFoodOrder;
    [SerializeField] Image _imageFillBar;

    // Image food ingredients
    [SerializeField] List<Image> _imageFood;
    // Image type Cook
    [SerializeField] Image _imageIconTypeCook;

    // Box
    [SerializeField] Image _imageBoxSoup;
    [SerializeField] List<Image> _imageBoxOther;

    // Color
    private string _hexStart = "#25FF09";
    private string _hexMid = "#FFF709";
    private string _hexEnd = "#FF3409";
    Color cStart, cMid, cEnd;

    // Animator
    private static int _HAS_ANI_ISTIMEOUT = Animator.StringToHash("isTimeOut");
    private static int _HAS_ANI_ISDESRTOY = Animator.StringToHash("isDestroy");
    private static int _HAS_ANI_ISCOMPLETED = Animator.StringToHash("isCompleted");
    private Animator _ani;
    private OrderSelected _selectedOrder;
    private OrderManager _manager;
    private FoodRandom _foodRandom;

    private bool _isCompleted = false;

    private void Awake()
    {
        _selectedOrder = GetComponent<OrderSelected>();
        _ani = GetComponentInChildren<Animator>();
        _ani.enabled = false;
        SetUpColor();
    }

    private void OnDisable()
    {
        _ani.enabled = false;
    }

    private void SetUpColor()
    {
        ColorUtility.TryParseHtmlString(_hexStart, out cStart);
        ColorUtility.TryParseHtmlString(_hexMid, out cMid);
        ColorUtility.TryParseHtmlString(_hexEnd, out cEnd);
    }    

    public void Init(FoodRandom foodRandom, OrderManager manager)
    {
        _ani.enabled = true;
        _manager = manager;
        _foodRandom = foodRandom;
        SetUp();
        ShowOnOrder(foodRandom);
        StartCoroutine(TimeNeedCookdDone(foodRandom));
    }
    private void SetUp()
    {
        _isCompleted = false;
        _FoodCompletedObj.SetActive(false);
        _imageBoxSoup.gameObject.SetActive(false);
        foreach (var food in _imageBoxOther) food.gameObject.SetActive(false);
        foreach (var food in _imageFood) food.gameObject.SetActive(false);
    }

    private void ShowOnOrder(FoodRandom foodRandom)
    {
        _imageFoodOrder.gameObject.SetActive(true);
        _imageFoodOrder.sprite = foodRandom.spriteFood;
        if (foodRandom.kitchenType != KitchenType.Plate) OneTypeFood(foodRandom);
        else MultiTypeFood(foodRandom);

    }    

    private void OneTypeFood(FoodRandom foodRandom)
    {
        IngredientRandom ingredient = foodRandom.ingredients[0];
        for (int i = 0; i < ingredient.amount; i += 1)
        {
            _imageFood[i].sprite = ingredient.spriteIngredient;
            _imageFood[i].gameObject.SetActive(true);
        }
        _imageBoxSoup.gameObject.SetActive(true);
    }

    private void MultiTypeFood(FoodRandom foodRandom)
    {
        int index = 0;
        int count = foodRandom.ingredients.Count;
        for(int i = 0; i < count; i++)
        {
            var ing = foodRandom.ingredients[i];
            if(CheckFoodCantShow(ing.foodType)) continue;
            _imageFood[index].sprite = ing.spriteIngredient;
            _imageFood[index].gameObject.SetActive(true);
            OrderOtherBox(ing.foodType, index);
            index += 1;
        }

    }

    private bool CheckFoodCantShow(FoodType foodType)
    {
        foreach(var food in _listFoodNotShow.icons)
        {
            if(food.foodType == foodType) return true;
        }
        return false;
    }

    private void OrderOtherBox(FoodType foodType, int index)
    {
        Sprite sprite = CheckSpriteTypeFoodNeedCooked(foodType);
        if(sprite == null)
        {
            _imageBoxOther[index + 1].gameObject.SetActive(true);  
        }else
        {
            _imageIconTypeCook.sprite = sprite;
            _imageBoxOther[0].gameObject.SetActive(true);
        }    
    }


    private Sprite CheckSpriteTypeFoodNeedCooked(FoodType foodType)
    {
        foreach (var food in _dataTypeCook.icons)
        {
            if (food.foodType == foodType)
            {
               return food._spriteCook;
            } 
        }
        return null;   
    }


    private IEnumerator TimeNeedCookdDone(FoodRandom foodRandom)
    {
        float time = 0;
        while(time < foodRandom.timeCookDone)
        {
            time += Time.deltaTime;
            float t = time / foodRandom.timeCookDone;
            _imageFillBar.fillAmount = Mathf.Lerp(1, 0, t);
            MakeColor(t);
            PlayAniamtion(t);
            if(_isCompleted) yield break;
            yield return null;
        }       
        StartCoroutine(WaitTimeDestroy(_HAS_ANI_ISDESRTOY));
    }

    private IEnumerator WaitTimeDestroy(int ani, float duration = 1f) 
    {
        _ani.SetTrigger(ani);
        yield return new WaitForSeconds(duration);
        _manager.RemoveOrderForList(this);
        PoolManager.Instance.Despawner(gameObject);
    }

    private void MakeColor(float time)
    {
        if(time < 0.5f)
        {
            float t = time / 0.5f;
            _imageFillBar.color = Color.Lerp(cStart, cMid, t);
        }else
        {
            float t = (time - 0.5f) / 0.5f;
            _imageFillBar.color = Color.Lerp(cMid, cEnd, t);
        }    
    }    

    private void PlayAniamtion(float t)
    {
        if(t < 0.8f) return;
        _ani.SetBool(_HAS_ANI_ISTIMEOUT, true);
    }    

    // =========== Service =============

    public void SetSelectedByBot(Sprite spriteBot) => _selectedOrder.OnSelectedByBot(spriteBot);
    public bool IsSelected() => _selectedOrder.IsSelected();
    public KitchenType GetKitchenType() => _foodRandom.kitchenType;
    public List<FoodType> GetFoods()
    {
        List<FoodType> foods = new();
        foreach(IngredientRandom ingredient in _foodRandom.ingredients)
        {
            for(int i = 0; i < ingredient.amount; i++) foods.Add(ingredient.foodType);
        }
        return foods;
    }
            
    public void OrderCompleted()
    {
        _isCompleted = true;
        _FoodCompletedObj.SetActive(true);
        StartCoroutine(WaitTimeDestroy(_HAS_ANI_ISCOMPLETED, 1.5f));
    }

}
