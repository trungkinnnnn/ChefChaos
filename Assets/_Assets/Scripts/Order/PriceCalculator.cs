public class PriceCalculator
{
    private float _discountRate;

    public PriceCalculator(float discountRate)
    {
        _discountRate = discountRate;
    }

    
    // =============== service =============
    public void UpdateDiscountRate(float discountRate)
    {
        _discountRate = discountRate;
    }

    public int CalculateDiscountPrice(int basePrice)
    {
        return (int)(basePrice - basePrice * _discountRate);
    }

}