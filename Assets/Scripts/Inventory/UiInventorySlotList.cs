using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventorySlotList : MonoBehaviour
{
    public UiInventorySlot prefab;
    public ScrollRect scrollRect;
    public UiItemInfo uiItemInfo;

    [Header("Capacity")]
    [SerializeField]
    private int maxSlots = 20;

    [SerializeField]
    private TextMeshProUGUI capacityText;

    [Header("Drop")]
    [SerializeField]
    private DropManager dropManager;

    public int MaxSlots => maxSlots;
    public bool IsFull => slotDataList.Count >= maxSlots;

    private List<UiInventorySlot> slotList = new List<UiInventorySlot>();

    private List<(ItemAsset asset, int amount)> slotDataList = new List<(ItemAsset, int)>();
    public List<(ItemAsset asset, int amount)> SlotDataList => slotDataList;

    private int selectedSlotIndex = -1;
    public int SelectedSlotIndex => selectedSlotIndex;

    private void Awake()
    {
        uiItemInfo.SetEmpty();
        UpdateCapacityText();
    }

    private bool TryAddItemData(ItemAsset asset)
    {
        int stackMax = asset.Data.StackMax;

        for (int i = 0; i < slotDataList.Count; i++)
        {
            if (slotDataList[i].asset.ItemID == asset.ItemID && slotDataList[i].amount < stackMax)
            {
                slotDataList[i] = (slotDataList[i].asset, slotDataList[i].amount + 1);
                return true;
            }
        }

        if (IsFull)
            return false;

        slotDataList.Add((asset, 1));
        return true;
    }

    public bool AddItem(ItemAsset asset)
    {
        if (asset.Data == null)
            asset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(asset.ItemID);

        bool success = TryAddItemData(asset);
        UpdateSlots();
        return success;
    }

    public int AddItem(ItemAsset asset, int amount)
    {
        if (asset.Data == null)
            asset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(asset.ItemID);

        int moved = 0;
        for (int i = 0; i < amount; i++)
        {
            if (!TryAddItemData(asset))
                break;
            moved++;
        }

        UpdateSlots();
        return moved;
    }

    public void RemoveItem()
    {
        RemoveItem(1);
    }

    public void RemoveItem(int amount)
    {
        if (selectedSlotIndex == -1)
            return;

        var entry = slotDataList[selectedSlotIndex];
        int newAmount = entry.amount - amount;

        if (newAmount <= 0)
            slotDataList.RemoveAt(selectedSlotIndex);
        else
            slotDataList[selectedSlotIndex] = (entry.asset, newAmount);

        selectedSlotIndex = -1;
        UpdateSlots();
    }

    public void Clear()
    {
        slotDataList.Clear();
        selectedSlotIndex = -1;
        UpdateSlots();
    }

    public ItemAsset GetSelectedItem()
    {
        if (selectedSlotIndex == -1)
            return null;
        return slotDataList[selectedSlotIndex].asset;
    }

    public int GetTotalAmount(string itemID)
    {
        int total = 0;
        foreach (var entry in slotDataList)
            if (entry.asset.ItemID == itemID)
                total += entry.amount;
        return total;
    }

    public void UpdateSlots()
    {
        int count = slotDataList.Count;

        if (slotList.Count < count)
        {
            for (int i = slotList.Count; i < count; i++)
            {
                var slot = Instantiate(prefab, scrollRect.content);
                slot.gameObject.SetActive(false);
                slotList.Add(slot);
            }
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            if (i < count)
            {
                int capturedIndex = i;
                slotList[i].slotIndex = i;
                slotList[i].button.onClick.RemoveAllListeners();
                slotList[i]
                    .button.onClick.AddListener(() =>
                    {
                        if (selectedSlotIndex == capturedIndex)
                        {
                            slotList[capturedIndex].SetNormal();
                            selectedSlotIndex = -1;
                            uiItemInfo.SetEmpty();
                            return;
                        }

                        if (selectedSlotIndex != -1 && selectedSlotIndex < slotList.Count)
                            slotList[selectedSlotIndex].SetNormal();

                        selectedSlotIndex = capturedIndex;
                        slotList[capturedIndex].SetSelected();
                        uiItemInfo.SetItem(slotDataList[capturedIndex].asset);
                    });
                slotList[i].SetItem(slotDataList[i].asset, slotDataList[i].amount);
                slotList[i].SetNormal();
            }
            else
            {
                slotList[i].SetEmpty();
            }
        }

        selectedSlotIndex = -1;
        uiItemInfo.SetEmpty();
        UpdateCapacityText();
    }

    public void SetSelectedIndex(int index)
    {
        selectedSlotIndex = index;
    }

    public void RemoveItemByAsset(ItemAsset asset, int amount)
    {
        int remaining = amount;

        for (int i = slotDataList.Count - 1; i >= 0; i--)
        {
            if (remaining <= 0)
                break;

            if (slotDataList[i].asset == asset)
            {
                int slotAmount = slotDataList[i].amount;

                if (slotAmount <= remaining)
                {
                    remaining -= slotAmount;
                    slotDataList.RemoveAt(i);
                }
                else
                {
                    slotDataList[i] = (slotDataList[i].asset, slotAmount - remaining);
                    remaining = 0;
                }
            }
        }

        UpdateSlots();
    }

    private void UpdateCapacityText()
    {
        if (capacityText == null)
            return;
        capacityText.text = $"{slotDataList.Count} / {maxSlots}";
    }

    public void OnClickDrop()
    {
        if (selectedSlotIndex == -1)
            return;

        int amount = slotDataList[selectedSlotIndex].amount;
        dropManager.OpenPopup(amount);
    }
}
