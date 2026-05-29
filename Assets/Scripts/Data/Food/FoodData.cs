public enum FoodEffectType
{
    Hunger,
    Hp,
}

public class FoodData
{
    public string FoodID { get; set; }
    public string DisplayName { get; set; }
    public FoodEffectType EffectType { get; set; }
    public int Value { get; set; }
}
