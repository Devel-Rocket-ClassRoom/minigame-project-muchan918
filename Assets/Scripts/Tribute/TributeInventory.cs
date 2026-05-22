using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TributeInventory : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public UiInventorySlot prefab;
    public ScrollRect scrollRect;
    public UiSubmitPanel submitPanel;

    private List<UiInventorySlot> slotList = new List<UiInventorySlot>();
    private List<(ItemAsset asset, int amount)> slotDataList;

    private int selectedSlotIndex = -1;

    private void Awake()
    {
        slotDataList = playerInventory.SlotList.SlotDataList;
        UpdateSlots();
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
                            submitPanel.OnClickPlus();
                            return;
                        }

                        if (selectedSlotIndex != -1)
                            slotList[selectedSlotIndex].SetNormal();

                        selectedSlotIndex = capturedIndex;
                        slotList[capturedIndex].SetSelected();
                    });
                slotList[i].SetItem(slotDataList[i].asset, slotDataList[i].amount);
                slotList[i].SetGray();
            }
            else
            {
                slotList[i].SetEmpty();
            }
        }
    }

    public void RemoveItems(Dictionary<int, int> submitDict)
    {
        foreach (var kvp in submitDict)
        {
            var entry = slotDataList[kvp.Key];
            slotDataList[kvp.Key] = (entry.asset, entry.amount - kvp.Value);
        }

        for (int i = slotDataList.Count - 1; i >= 0; i--)
        {
            if (slotDataList[i].amount <= 0)
                slotDataList.RemoveAt(i);
        }
    }

    public int GetTotalAmount(string itemID)
    {
        int total = 0;
        foreach (var entry in slotDataList)
            if (entry.asset.ItemID == itemID)
                total += entry.amount;
        return total;
    }

    public void HighlightItem(string itemID)
    {
        selectedSlotIndex = -1;

        for (int i = 0; i < slotDataList.Count; i++)
        {
            if (slotDataList[i].asset.ItemID == itemID)
                slotList[i].SetNormal();
            else
                slotList[i].SetGray();
        }
    }

    public int GetSelectedSlotIndex()
    {
        return selectedSlotIndex;
    }

    public void Reset()
    {
        selectedSlotIndex = -1;
        for (int i = 0; i < slotDataList.Count; i++)
            slotList[i].SetGray();
    }
}
