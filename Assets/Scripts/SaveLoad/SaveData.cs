using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData
{
    public int Version { get; set; }
    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public int CurrentDay { get; set; } = 1;
    public int HungerEmptyCount { get; set; } = 0;
    public int CurrentHp { get; set; } = 100;

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        return new SaveDataV2
        {
            Version = 2,
            CurrentDay = CurrentDay,
            HungerEmptyCount = HungerEmptyCount,
            CurrentHp = CurrentHp,
            Inventory = new(),
            Storage = new(),
        };
    }
}

public class SaveDataV2 : SaveDataV1
{
    public List<SaveSlotData> Inventory { get; set; } = new();
    public List<SaveSlotData> Storage { get; set; } = new();

    public SaveDataV2()
    {
        Version = 2;
    }

    public override SaveData VersionUp()
    {
        return new SaveDataV3
        {
            Version = 3,
            CurrentDay = CurrentDay,
            HungerEmptyCount = HungerEmptyCount,
            CurrentHp = CurrentHp,
            Inventory = Inventory,
            Storage = Storage,
            TributeLevel = 0,
            CurrentTributeLevel = 0,
            TributeRequirementID = "",
            TributeSubmitted = new(),
        };
    }
}

public class SaveDataV3 : SaveDataV2
{
    public int TributeLevel { get; set; } = 0;
    public int CurrentTributeLevel { get; set; } = 0;
    public string TributeRequirementID { get; set; } = "";
    public List<int> TributeSubmitted { get; set; } = new();

    public SaveDataV3()
    {
        Version = 3;
    }

    public override SaveData VersionUp()
    {
        return new SaveDataV4
        {
            Version = 4,
            CurrentDay = CurrentDay,
            HungerEmptyCount = HungerEmptyCount,
            CurrentHp = CurrentHp,
            Inventory = Inventory,
            Storage = Storage,
            TributeLevel = TributeLevel,
            CurrentTributeLevel = CurrentTributeLevel,
            TributeRequirementID = TributeRequirementID,
            TributeSubmitted = TributeSubmitted,
            UpgradeWorkbench = 0,
            UpgradeStorage = 0,
            UpgradeInventory = 0,
            UpgradeAnimal = 0,
            UpgradeResource = 0,
            UpgradeCauldron = 0,
        };
    }
}

public class SaveDataV4 : SaveDataV3
{
    public int UpgradeWorkbench { get; set; } = 0;
    public int UpgradeStorage { get; set; } = 0;
    public int UpgradeInventory { get; set; } = 0;
    public int UpgradeAnimal { get; set; } = 0;
    public int UpgradeResource { get; set; } = 0;
    public int UpgradeCauldron { get; set; } = 0;

    public SaveDataV4()
    {
        Version = 4;
    }

    public override SaveData VersionUp()
    {
        return new SaveDataV5
        {
            Version = 5,
            CurrentDay = CurrentDay,
            HungerEmptyCount = HungerEmptyCount,
            CurrentHp = CurrentHp,
            Inventory = Inventory,
            Storage = Storage,
            TributeLevel = TributeLevel,
            CurrentTributeLevel = CurrentTributeLevel,
            TributeRequirementID = TributeRequirementID,
            TributeSubmitted = TributeSubmitted,
            UpgradeWorkbench = UpgradeWorkbench,
            UpgradeStorage = UpgradeStorage,
            UpgradeInventory = UpgradeInventory,
            UpgradeAnimal = UpgradeAnimal,
            UpgradeResource = UpgradeResource,
            UpgradeCauldron = UpgradeCauldron,
            EquipHat = null,
            EquipTop = null,
            EquipBottom = null,
            EquipShoes = null,
            EquipWeaponRight = null,
        };
    }
}

public class SaveDataV5 : SaveDataV4
{
    public string EquipHat { get; set; } = null;
    public string EquipTop { get; set; } = null;
    public string EquipBottom { get; set; } = null;
    public string EquipShoes { get; set; } = null;
    public string EquipWeaponRight { get; set; } = null;

    public SaveDataV5()
    {
        Version = 5;
    }

    public override SaveData VersionUp()
    {
        return new SaveDataV6
        {
            Version = 6,
            CurrentDay = CurrentDay,
            HungerEmptyCount = HungerEmptyCount,
            CurrentHp = CurrentHp,
            Inventory = Inventory,
            Storage = Storage,
            TributeLevel = TributeLevel,
            CurrentTributeLevel = CurrentTributeLevel,
            TributeRequirementID = TributeRequirementID,
            TributeSubmitted = TributeSubmitted,
            UpgradeWorkbench = UpgradeWorkbench,
            UpgradeStorage = UpgradeStorage,
            UpgradeInventory = UpgradeInventory,
            UpgradeAnimal = UpgradeAnimal,
            UpgradeResource = UpgradeResource,
            UpgradeCauldron = UpgradeCauldron,
            EquipHat = EquipHat,
            EquipTop = EquipTop,
            EquipBottom = EquipBottom,
            EquipShoes = EquipShoes,
            EquipWeaponRight = EquipWeaponRight,
            MapSeed = 0,
            DestroyedResources = new(),
            DeadAnimals = new(),
        };
    }
}

public class SaveDataV6 : SaveDataV5
{
    public int MapSeed { get; set; } = 0;
    public List<Vector2> DestroyedResources { get; set; } = new();
    public List<Vector2> DeadAnimals { get; set; } = new();

    public SaveDataV6()
    {
        Version = 6;
    }

    public override SaveData VersionUp()
    {
        return new SaveDataV7
        {
            Version = 7,
            CurrentDay = CurrentDay,
            HungerEmptyCount = HungerEmptyCount,
            CurrentHp = CurrentHp,
            Inventory = Inventory,
            Storage = Storage,
            TributeSubmitted = TributeSubmitted,
            TributeRequirementIndex = 0,
            UpgradeWorkbench = UpgradeWorkbench,
            UpgradeStorage = UpgradeStorage,
            UpgradeInventory = UpgradeInventory,
            UpgradeAnimal = UpgradeAnimal,
            UpgradeResource = UpgradeResource,
            UpgradeCauldron = UpgradeCauldron,
            EquipHat = EquipHat,
            EquipTop = EquipTop,
            EquipBottom = EquipBottom,
            EquipShoes = EquipShoes,
            EquipWeaponRight = EquipWeaponRight,
            MapSeed = MapSeed,
            DestroyedResources = DestroyedResources,
            DeadAnimals = DeadAnimals,
        };
    }
}

public class SaveDataV7 : SaveDataV6
{
    public int TributeRequirementIndex { get; set; } = 0;

    public SaveDataV7()
    {
        Version = 7;
    }

    public override SaveData VersionUp()
    {
        return this;
    }
}
