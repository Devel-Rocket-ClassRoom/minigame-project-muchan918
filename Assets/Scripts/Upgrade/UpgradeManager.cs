using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Player Systems")]
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private PlayerAction playerAction;

    [SerializeField]
    private DayNightCycle dayNightCycle;

    [SerializeField]
    private StorageInventory storageInventory;

    [SerializeField]
    private UiInventorySlotList inventorySlotList;

    [Header("Auto Upgrade Systems")]
    [SerializeField]
    private UiCraftSlotList craftSlotList;

    [SerializeField]
    private UiCauldronSlotList cauldronSlotList;

    [Header("Player")]
    [SerializeField]
    private PlayerInventory playerInventory;

    [Header("Auto Upgrade Days")]
    [SerializeField]
    private int[] autoUpgradeDays = { 14, 21 };

    private Dictionary<UpgradeType, IUpgradeable> upgradeTargets;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        upgradeTargets = new Dictionary<UpgradeType, IUpgradeable>
        {
            { UpgradeType.MoveSpeed, playerMovement },
            { UpgradeType.AttackSpeed, playerAction },
            { UpgradeType.DayDuration, dayNightCycle },
            { UpgradeType.Inventory, inventorySlotList },
            { UpgradeType.Storage, storageInventory },
            { UpgradeType.Animal, GetComponent<AnimalGenerator>() },
            { UpgradeType.Resource, GetComponent<ResourceGenerator>() },
            { UpgradeType.Workbench, craftSlotList },
            { UpgradeType.Cauldron, cauldronSlotList },
        };
    }

    public int GetLevel(UpgradeType type) => upgradeTargets[type].Level;

    public bool IsMaxLevel(UpgradeAsset asset) =>
        upgradeTargets[asset.type].Level >= asset.MaxLevel;

    public bool CanAfford(UpgradeAsset asset)
    {
        if (IsMaxLevel(asset))
            return false;

        int level = upgradeTargets[asset.type].Level;
        foreach (var ingredient in asset.costPerLevel[level - 1].ingredients)
        {
            int owned = playerInventory.SlotList.GetTotalAmount(ingredient.item.ItemID);
            if (owned < ingredient.amount)
                return false;
        }
        return true;
    }

    public bool Upgrade(UpgradeAsset asset)
    {
        if (!CanAfford(asset))
            return false;

        int level = upgradeTargets[asset.type].Level;
        foreach (var ingredient in asset.costPerLevel[level - 1].ingredients)
            playerInventory.SlotList.RemoveItemByAsset(ingredient.item, ingredient.amount);

        upgradeTargets[asset.type].Upgrade();
        return true;
    }

    public void CheckAutoUpgrade(int currentDay)
    {
        foreach (int day in autoUpgradeDays)
        {
            if (currentDay == day)
            {
                upgradeTargets[UpgradeType.Animal].Upgrade();
                upgradeTargets[UpgradeType.Resource].Upgrade();
                upgradeTargets[UpgradeType.Workbench].Upgrade();
                upgradeTargets[UpgradeType.Cauldron].Upgrade();
                Debug.Log($"Day {currentDay} 자동 업그레이드 완료");
                break;
            }
        }
    }

    public void ForceUpgrade(UpgradeType type)
    {
        upgradeTargets[type].Upgrade();
    }
}
