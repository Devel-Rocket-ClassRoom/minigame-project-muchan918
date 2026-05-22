using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TributeInventory : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public UiInventorySlot prefab;
    public ScrollRect scrollRect;

    private List<UiInventorySlot> slotList = new List<UiInventorySlot>();
    private List<(ItemAsset asset, int amount)> slotDataList;

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
                slotList[i].slotIndex = i;
                slotList[i].button.onClick.RemoveAllListeners();
                slotList[i].SetItem(slotDataList[i].asset, slotDataList[i].amount);
            }
            else
            {
                slotList[i].SetEmpty();
            }
        }
    }
}
