using System.Collections.Generic;
using UnityEngine;

public class UiTributeSlotList : MonoBehaviour
{
    public UiTributeSlot prefab;

    private List<UiTributeSlot> slotList = new List<UiTributeSlot>();

    private int selectedSlotIndex = -1;
    public int SelectedSlotIndex => selectedSlotIndex;

    public void Setup(TributeRequirement requirement)
    {
        foreach (var slot in slotList)
            Destroy(slot.gameObject);
        slotList.Clear();

        selectedSlotIndex = -1;

        int index = 0;
        foreach (var entry in requirement.requiredItems)
        {
            var slot = Instantiate(prefab, transform);
            slot.slotIndex = index;
            slot.SetSlot(entry.item, entry.amount);

            int capturedIndex = index;
            slot.button.onClick.AddListener(() => selectedSlotIndex = capturedIndex);

            slotList.Add(slot);
            index++;
        }
    }

    public UiTributeSlot GetSelectedSlot()
    {
        if (selectedSlotIndex == -1)
            return null;
        return slotList[selectedSlotIndex];
    }

    public bool IsAllComplete()
    {
        foreach (var slot in slotList)
            if (!slot.IsComplete)
                return false;
        return true;
    }
}
