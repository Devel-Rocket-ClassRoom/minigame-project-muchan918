public enum EquipSlotType
{
    Hat,
    Top,
    Bottom,
    Shoes,
    WeaponRight,
}

public enum EquipType
{
    None,
    Axe,
    Sword,
}

public class EquipmentData
{
    public string EquipmentID { get; set; }
    public EquipSlotType SlotType { get; set; }
    public string PartsName { get; set; }
    public int PartsIndex { get; set; }
    public EquipType EquipType { get; set; }
    public int Value { get; set; }
    public string TargetTag { get; set; }
}
