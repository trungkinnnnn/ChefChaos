public class BotStep
{
    public StepTask stepTask;
    public StationType targetStation;
    public FoodType requiredFood;
    public KitchenType kitchenType;
    public float timeCooking;

    public BotStep(StepTask stepTask, StationType targetStation, FoodType requiredFood, KitchenType kitchenType, float timeCooking)
    {
        this.stepTask = stepTask;
        this.targetStation = targetStation;
        this.requiredFood = requiredFood;
        this.kitchenType = kitchenType;
        this.timeCooking = timeCooking;
    }

}