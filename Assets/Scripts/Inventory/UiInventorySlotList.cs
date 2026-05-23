using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventorySlotList : MonoBehaviour
{
    public UiInventorySlot prefab;
    public ScrollRect scrollRect;
    public UiItemInfo uiItemInfo;

    private List<UiInventorySlot> slotList = new List<UiInventorySlot>();

    private List<(ItemAsset asset, int amount)> slotDataList = new List<(ItemAsset, int)>();
    public List<(ItemAsset asset, int amount)> SlotDataList => slotDataList;

    private int selectedSlotIndex = -1;
    public int SelectedSlotIndex => selectedSlotIndex;

    private void Awake()
    {
        uiItemInfo.SetEmpty();
    }

    public void AddItem(ItemAsset asset)
    {
        if (asset.Data == null)
            asset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(asset.ItemID);

        int stackMax = asset.Data.StackMax;

        // 같은 ItemID 중 마지막 슬롯 찾기
        int lastIndex = -1;
        for (int i = slotDataList.Count - 1; i >= 0; i--)
        {
            if (slotDataList[i].asset.ItemID == asset.ItemID)
            {
                lastIndex = i;
                break;
            }
        }

        if (lastIndex != -1 && slotDataList[lastIndex].amount < stackMax)
        {
            // 마지막 슬롯에 수량 추가
            var entry = slotDataList[lastIndex];
            slotDataList[lastIndex] = (entry.asset, entry.amount + 1);
        }
        else
        {
            // 새 슬롯 추가 (순서 그대로 뒤에 붙음)
            slotDataList.Add((asset, 1));
        }

        UpdateSlots();
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

    private void UpdateSlots()
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
                        selectedSlotIndex = capturedIndex;
                        uiItemInfo.SetItem(slotDataList[capturedIndex].asset);
                    });
                slotList[i].SetItem(slotDataList[i].asset, slotDataList[i].amount);
            }
            else
            {
                slotList[i].SetEmpty();
            }
        }

        selectedSlotIndex = -1;
        uiItemInfo.SetEmpty();
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
                    // 슬롯 전체 제거
                    remaining -= slotAmount;
                    slotDataList.RemoveAt(i);
                }
                else
                {
                    // 일부만 차감
                    slotDataList[i] = (slotDataList[i].asset, slotAmount - remaining);
                    remaining = 0;
                }
            }
        }

        UpdateSlots();
    }
}
