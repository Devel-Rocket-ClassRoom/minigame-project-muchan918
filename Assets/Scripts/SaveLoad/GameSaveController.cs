using System.Collections.Generic;
using UnityEngine;

public class GameSaveController : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public PlayerHealth playerHealth;
    public PlayerHunger playerHunger;
    public PlayerInventory playerInventory;
    public StorageInventory storageInventory;
    public TributeEvent tributeEvent;
    public UpgradeManager upgradeManager;
    public PlayerEquipment playerEquipment;
    public UiEquipPanel uiEquipPanel;

    private void Start()
    {
        ApplySaveData();
    }

    public void SaveGame()
    {
        var data = SaveLoadManager.Data;

        // V1
        data.CurrentDay = dayNightCycle.CurrentDay;
        data.HungerEmptyCount = dayNightCycle.HungerEmptyCount;
        data.CurrentHp = playerHealth.CurrentHp;

        // V2
        data.Inventory = GetSlotDataList(playerInventory.SlotList.SlotDataList);
        data.Storage = GetSlotDataList(storageInventory.SlotDataList);

        // V3
        data.TributeLevel = tributeEvent.TributeLevel;
        data.CurrentTributeLevel = tributeEvent.CurrentEventLevel;
        data.TributeRequirementID = tributeEvent.CurrentRequirementID;
        data.TributeSubmitted = tributeEvent.tributeSlotList.GetSubmittedList();

        // V4
        data.UpgradeWorkbench = upgradeManager.GetLevel(UpgradeType.Workbench);
        data.UpgradeStorage = upgradeManager.GetLevel(UpgradeType.Storage);
        data.UpgradeInventory = upgradeManager.GetLevel(UpgradeType.Inventory);
        data.UpgradeAnimal = upgradeManager.GetLevel(UpgradeType.Animal);
        data.UpgradeResource = upgradeManager.GetLevel(UpgradeType.Resource);
        data.UpgradeCauldron = upgradeManager.GetLevel(UpgradeType.Cauldron);

        // V5
        data.EquipHat = playerEquipment.GetEquippedItem(EquipSlotType.Hat)?.ItemID;
        data.EquipTop = playerEquipment.GetEquippedItem(EquipSlotType.Top)?.ItemID;
        data.EquipBottom = playerEquipment.GetEquippedItem(EquipSlotType.Bottom)?.ItemID;
        data.EquipShoes = playerEquipment.GetEquippedItem(EquipSlotType.Shoes)?.ItemID;
        data.EquipWeaponRight = playerEquipment.GetEquippedItem(EquipSlotType.WeaponRight)?.ItemID;

        SaveLoadManager.Save();
    }

    private void ApplySaveData()
    {
        var data = SaveLoadManager.Data;

        // V1
        dayNightCycle.SetDay(data.CurrentDay);
        dayNightCycle.SetHungerEmptyCount(data.HungerEmptyCount);
        playerHealth.SetHealth(data.CurrentHp);

        // V2
        RestoreInventory(playerInventory, data.Inventory);
        RestoreStorageInventory(storageInventory, data.Storage);

        // V3
        if (!string.IsNullOrEmpty(data.TributeRequirementID))
        {
            tributeEvent.SetRequirementByID(
                data.TributeLevel,
                data.CurrentTributeLevel,
                data.TributeRequirementID
            );
            tributeEvent.tributeSlotList.RestoreSubmitted(data.TributeSubmitted);
        }

        // V4
        ApplyUpgradeLevel(UpgradeType.Workbench, data.UpgradeWorkbench);
        ApplyUpgradeLevel(UpgradeType.Storage, data.UpgradeStorage);
        ApplyUpgradeLevel(UpgradeType.Inventory, data.UpgradeInventory);
        ApplyUpgradeLevel(UpgradeType.Animal, data.UpgradeAnimal);
        ApplyUpgradeLevel(UpgradeType.Resource, data.UpgradeResource);
        ApplyUpgradeLevel(UpgradeType.Cauldron, data.UpgradeCauldron);

        // V5
        RestoreEquip(EquipSlotType.Hat, data.EquipHat);
        RestoreEquip(EquipSlotType.Top, data.EquipTop);
        RestoreEquip(EquipSlotType.Bottom, data.EquipBottom);
        RestoreEquip(EquipSlotType.Shoes, data.EquipShoes);
        RestoreEquip(EquipSlotType.WeaponRight, data.EquipWeaponRight);
    }

    private List<SaveSlotData> GetSlotDataList(List<(ItemAsset asset, int amount)> slots)
    {
        var result = new List<SaveSlotData>();
        foreach (var (asset, amount) in slots)
            result.Add(new SaveSlotData(asset.ItemID, amount));
        return result;
    }

    private void RestoreInventory(PlayerInventory inventory, List<SaveSlotData> slots)
    {
        inventory.SlotList.Clear();
        foreach (var slot in slots)
        {
            var asset = Resources.Load<ItemAsset>($"ScriptableObjects/Items/{slot.ItemID}");
            if (asset == null)
            {
                Debug.LogWarning($"[Load] ItemAsset 못 찾음: {slot.ItemID}");
                continue;
            }
            inventory.SlotList.LoadSlot(asset, slot.Amount);
        }
    }

    private void RestoreStorageInventory(StorageInventory inventory, List<SaveSlotData> slots)
    {
        foreach (var slot in slots)
        {
            var asset = Resources.Load<ItemAsset>($"ScriptableObjects/Items/{slot.ItemID}");
            if (asset == null)
            {
                Debug.LogWarning($"[Load] ItemAsset 못 찾음: {slot.ItemID}");
                continue;
            }
            inventory.LoadSlot(asset, slot.Amount);
        }
    }

    private void ApplyUpgradeLevel(UpgradeType type, int targetLevel)
    {
        int current = upgradeManager.GetLevel(type);
        for (int i = current; i < targetLevel; i++)
            upgradeManager.ForceUpgrade(type);
    }

    private void RestoreEquip(EquipSlotType slot, string itemID)
    {
        if (string.IsNullOrEmpty(itemID))
            return;

        var itemAsset = Resources.Load<ItemAsset>($"ScriptableObjects/Items/{itemID}");
        if (itemAsset == null)
        {
            Debug.LogWarning($"[Load] ItemAsset 못 찾음: {itemID}");
            return;
        }

        var equipData = DataTableManager.Get<EquipmentTable>("EquipmentTable").Get(itemID);
        if (equipData == null)
        {
            Debug.LogWarning($"[Load] EquipmentData 못 찾음: {itemID}");
            return;
        }

        PlayerEquipment.Instance.Equip(equipData, itemAsset);
        uiEquipPanel.Equip(equipData.SlotType, itemAsset);
    }
}
