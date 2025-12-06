using System.Collections.Generic;

public interface IPlate
{
    public PlateState GetStatePlate();
    public List<FoodType> GetFoodTypes();

    public void ResetPlate();
}