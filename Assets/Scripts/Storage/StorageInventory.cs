using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageInventory : MonoBehaviour, IUpgradeable
{
    public UiInventorySlot prefab;
    public ScrollRect scrollRect;
    public int Level { get; private set; }

    [Header("Capacity")]
    [SerializeField]
    private int maxSlots = 20;

    [SerializeField]
    private TextMeshProUGUI capacityText;

    public int MaxSlots => maxSlots;
    public bool IsFull => slotDataList.Count >= maxSlots;

    private List<UiInventorySlot> slotList = new List<UiInventorySlot>();
    private List<(ItemAsset asset, int amount)> slotDataList = new List<(ItemAsset, int)>();
    public List<(ItemAsset asset, int amount)> SlotDataList => slotDataList;

    private int selectedSlotIndex = -1;
    public int SelectedSlotIndex => selectedSlotIndex;

    public System.Action<int> OnSlotClicked;

    private void Awake()
    {
        UpdateCapacityText();
    }

    public void Upgrade()
    {
        Level++;
        maxSlots += 10;
        Debug.Log($"[Upgrade] 저장소 Lv {Level}");
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

    public int AddItem(ItemAsset asset, int amount = 1)
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

    private void UpdateCapacityText()
    {
        if (capacityText == null)
            return;
        capacityText.text = $"{slotDataList.Count} / {maxSlots}";
    }
}
