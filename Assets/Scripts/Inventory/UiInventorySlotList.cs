using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventorySlotList : MonoBehaviour
{
    public UiInventorySlot prefab;
    public ScrollRect scrollRect;
    public UiItemInfo uiItemInfo;

    private List<UiInventorySlot> slotList = new List<UiInventorySlot>();
    private List<ItemAsset> itemAssetList = new List<ItemAsset>();

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

        itemAssetList.Add(asset);
        UpdateSlots();
    }

    public void RemoveItem()
    {
        if (selectedSlotIndex == -1)
            return;

        itemAssetList.RemoveAt(selectedSlotIndex);
        selectedSlotIndex = -1;
        UpdateSlots();
    }

    public void Clear()
    {
        itemAssetList.Clear();
        selectedSlotIndex = -1;
        UpdateSlots();
    }

    public ItemAsset GetSelectedItem()
    {
        if (selectedSlotIndex == -1)
            return null;
        return itemAssetList[selectedSlotIndex];
    }

    private void UpdateSlots()
    {
        if (slotList.Count < itemAssetList.Count)
        {
            for (int i = slotList.Count; i < itemAssetList.Count; i++)
            {
                var slot = Instantiate(prefab, scrollRect.content);
                slot.slotIndex = i;
                slot.gameObject.SetActive(false);

                int capturedIndex = i;
                slot.button.onClick.AddListener(() =>
                {
                    selectedSlotIndex = capturedIndex;
                    uiItemInfo.SetItem(itemAssetList[capturedIndex]);
                });

                slotList.Add(slot);
            }
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            if (i < itemAssetList.Count)
            {
                slotList[i].gameObject.SetActive(true);
                slotList[i].SetItem(itemAssetList[i]);
            }
            else
            {
                slotList[i].gameObject.SetActive(false);
                slotList[i].SetEmpty();
            }
        }

        selectedSlotIndex = -1;
        uiItemInfo.SetEmpty();
    }
}
