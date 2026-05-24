using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageInventory : MonoBehaviour
{
    public UiInventorySlot prefab;
    public ScrollRect scrollRect;

    private List<UiInventorySlot> slotList = new List<UiInventorySlot>();
    private List<(ItemAsset asset, int amount)> slotDataList = new List<(ItemAsset, int)>();
    public List<(ItemAsset asset, int amount)> SlotDataList => slotDataList;

    private int selectedSlotIndex = -1;
    public int SelectedSlotIndex => selectedSlotIndex;

    public System.Action<int> OnSlotClicked;

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
                        int previousIndex = selectedSlotIndex;

                        if (selectedSlotIndex != -1)
                            slotList[selectedSlotIndex].SetNormal();

                        selectedSlotIndex = capturedIndex;
                        slotList[capturedIndex].SetSelected();

                        OnSlotClicked?.Invoke(previousIndex);
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
    }

    public void AddItem(ItemAsset asset, int amount = 1)
    {
        if (asset.Data == null)
            asset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(asset.ItemID);

        int stackMax = asset.Data.StackMax;

        for (int i = 0; i < amount; i++)
        {
            int lastIndex = -1;
            for (int j = 0; j < slotDataList.Count; j++)
            {
                if (
                    slotDataList[j].asset.ItemID == asset.ItemID
                    && slotDataList[j].amount < stackMax
                )
                {
                    lastIndex = j;
                    break;
                }
            }

            if (lastIndex != -1)
            {
                var entry = slotDataList[lastIndex];
                slotDataList[lastIndex] = (entry.asset, entry.amount + 1);
            }
            else
            {
                slotDataList.Add((asset, 1));
            }
        }

        UpdateSlots();
    }

    public ItemAsset GetSelectedAsset()
    {
        if (selectedSlotIndex == -1)
            return null;
        return slotDataList[selectedSlotIndex].asset;
    }

    public int GetSelectedAmount()
    {
        if (selectedSlotIndex == -1)
            return 0;
        return slotDataList[selectedSlotIndex].amount;
    }

    public void RemoveFromSelected(int amount)
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

    public void ResetSelection()
    {
        if (selectedSlotIndex != -1 && selectedSlotIndex < slotList.Count)
            slotList[selectedSlotIndex].SetNormal();
        selectedSlotIndex = -1;
    }
}
