using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    MoveSpeed,
    AttackSpeed,
    DayDuration,
    Inventory,
    Storage,

    // 자동 업그레이드
    Animal,
    Resource,
    Workbench,
    Cauldron,
}

[CreateAssetMenu(menuName = "7Days/UpgradeAsset")]
public class UpgradeAsset : ScriptableObject
{
    [System.Serializable]
    public class Ingredient
    {
        public ItemAsset item;
        public int amount;
    }

    [System.Serializable]
    public class Cost
    {
        public List<Ingredient> ingredients;
    }

    public UpgradeType type;
    public string displayName;
    public Sprite icon;

    public List<Cost> costPerLevel;

    public int MaxLevel => costPerLevel.Count + 1;
}
