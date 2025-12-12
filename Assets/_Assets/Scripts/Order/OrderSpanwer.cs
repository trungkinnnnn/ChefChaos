using System.Collections.Generic;
using UnityEngine;

public class OrderSpanwer
{
    private CookingRecipeDatabase _recipeDatabase;
    private int _maxFoodIngredient = 4;
    private int _lastIndex = -1;

    public OrderSpanwer(CookingRecipeDatabase recipeDatabase)
    {
        _recipeDatabase = recipeDatabase;
    }    

    private IngredientRandom AddIngredientRandomFood(IngredientRequiment ingredient)
    {
        return new IngredientRandom(ingredient.foodType, ingredient.amount, ingredient.foodSprite);
    }    

    private int RandomFoodOrder()
    {
        int index = 0;  
        do
        {
            index = Random.Range(0, _recipeDatabase.recipes.Count);
        }while(index == _lastIndex);
        _lastIndex = index;
        return index;
    }

    private bool RandomYesNo() => Random.value > 0.5f;

    // ============= Service ================
    public FoodRandom RandomIngredientFood()
    {
        int index = RandomFoodOrder();
        int foodCount = 0;

        FoodRandom foodRandom = new(_recipeDatabase.recipes[index].sprite,
                                    _recipeDatabase.recipes[index].type,
                                    new List<IngredientRandom>(), 
                                    _recipeDatabase.recipes[index].timeCookDone);

        foreach (var ingerdient in _recipeDatabase.recipes[index].ingredients)
        {

            if (ingerdient.isRequied)
            {
                foodRandom.ingredients.Add(AddIngredientRandomFood(ingerdient));
                foodCount += 1;
                continue;
            }
            if (RandomYesNo())
            {
                foodRandom.ingredients.Add(AddIngredientRandomFood(ingerdient));
                foodCount += 1;
            }

            if (foodCount == _maxFoodIngredient) break;
        }
        return foodRandom;
    }

}

public class FoodRandom
{
    public Sprite spriteFood;
    public KitchenType ObjType;
    public List<IngredientRandom> ingredients;
    public float timeCookDone;
    
    public FoodRandom(Sprite spriteFood, KitchenType ObjType, List<IngredientRandom> ingredients, float timeCookDone)
    {
        this.spriteFood = spriteFood;
        this.ObjType = ObjType;
        this.ingredients = ingredients;
        this.timeCookDone = timeCookDone;   
    }
}

public class IngredientRandom
{
    public FoodType foodType;
    public int amount;
    public Sprite spriteIngredient;

    public IngredientRandom(FoodType foodType, int amount, Sprite spriteIngredient)
    {
        this.foodType = foodType;
        this.amount = amount;
        this.spriteIngredient = spriteIngredient;
    }
}