public enum AnimalType
{
    Passive,
    Hostile,
}

public class AnimalData
{
    public string AnimalID { get; set; }
    public string DisplayName { get; set; }
    public int MaxHP { get; set; }
    public float MoveSpeed { get; set; }
    public AnimalType AnimalType { get; set; }
}
