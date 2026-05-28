public enum EquipSlotType
{
    Hat,
    Top,
    Bottom,
    Shoes,
    WeaponRight,
    WeaponBack,
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
    public EquipSlotType SlotType { get; set; } // 해제할 때 사용
    public string PartsName { get; set; } // 장착할 때 사용 (Axe, Top, Bottom...)
    public int PartsIndex { get; set; } // 장착할 때 사용
    public EquipType EquipType { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
}
