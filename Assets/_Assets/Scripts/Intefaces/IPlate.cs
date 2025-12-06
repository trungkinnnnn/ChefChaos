using System.Collections.Generic;

public interface IPlate
{
    public void SetDrityPlate();
    public PlateState GetStatePlate();
    public List<FoodType> GetFoodTypes();
    public void ResetPlate();
}