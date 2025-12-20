public class BotStep
{
    public StationType targetStation;
    public FoodType requiredFood;
    public KitchenType kitchenType;
    public float timeCooking;

    public BotStep(StationType targetStation, FoodType requiredFood, KitchenType kitchenType, float timeCooking)
    {
        this.targetStation = targetStation;
        this.requiredFood = requiredFood;
        this.kitchenType = kitchenType;
        this.timeCooking = timeCooking;
    }

}