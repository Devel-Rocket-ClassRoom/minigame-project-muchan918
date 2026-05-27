using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoragePlayerInventory : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public UiInventorySlot prefab;
    public ScrollRect scrollRect;

    private List<UiInventorySlot> slotList = new List<UiInventorySlot>();
    private List<(ItemAsset asset, int amount)> slotDataList;

    private int selectedSlotIndex = -1;
    public int SelectedSlotIndex => selectedSlotIndex;

    public System.Action<int> OnSlotClicked;

    [SerializeField]
    private TextMeshProUGUI capacityText;

    private void Awake()
    {
        slotDataList = playerInventory.SlotList.SlotDataList;
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
        UpdateCapacityText();
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

    public void ResetSelection()
    {
        if (selectedSlotIndex != -1 && selectedSlotIndex < slotList.Count)
            slotList[selectedSlotIndex].SetNormal();
        selectedSlotIndex = -1;
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

    private void UpdateCapacityText()
    {
        if (capacityText == null)
            return;
        capacityText.text = $"{slotDataList.Count} / {playerInventory.SlotList.MaxSlots}";
    }
}
