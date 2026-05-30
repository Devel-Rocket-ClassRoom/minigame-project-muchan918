using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Systems")]
    [SerializeField]
    private UiCraftSlotList craftSlotList;

    [SerializeField]
    private StorageInventory storageInventory;

    [SerializeField]
    private UiInventorySlotList inventorySlotList;

    [Header("Player")]
    [SerializeField]
    private PlayerInventory playerInventory;

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
            { UpgradeType.Workbench, craftSlotList },
            { UpgradeType.Storage, storageInventory },
            { UpgradeType.Inventory, inventorySlotList },
            { UpgradeType.Animal, GetComponent<AnimalGenerator>() },
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
        foreach (var ingredient in asset.costPerLevel[level].ingredients)
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

        foreach (var ingredient in asset.costPerLevel[level].ingredients)
            playerInventory.SlotList.RemoveItemByAsset(ingredient.item, ingredient.amount);

        upgradeTargets[asset.type].Upgrade();

        return true;
    }
}
