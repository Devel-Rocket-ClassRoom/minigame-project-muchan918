using System.Collections.Generic;
using UnityEngine;

public class UiEquipPanel : MonoBehaviour
{
    public List<UiEquipSlot> slots;
    public UiItemInfo uiItemInfo;

    private void Start()
    {
        foreach (var slot in slots)
        {
            var capturedSlot = slot;
            slot.button.onClick.AddListener(() => OnClickSlot(capturedSlot));
        }
    }

    public void Equip(EquipSlotType slotType, ItemAsset item)
    {
        var slot = GetSlot(slotType);
        if (slot == null)
            return;
        slot.SetItem(item);
    }

    public void UnEquip(EquipSlotType slotType)
    {
        var slot = GetSlot(slotType);
        if (slot == null)
            return;
        slot.SetEmpty();
    }

    private void OnClickSlot(UiEquipSlot slot)
    {
        if (!PlayerEquipment.Instance.IsEquipped(slot.slotType))
        {
            uiItemInfo.SetEmpty();
            return;
        }

        ItemAsset equippedItem = PlayerEquipment.Instance.GetEquippedItem(slot.slotType);
        uiItemInfo.SetEquippedItem(equippedItem, slot.slotType);
    }

    private UiEquipSlot GetSlot(EquipSlotType slotType)
    {
        foreach (var slot in slots)
            if (slot.slotType == slotType)
                return slot;
        return null;
    }
}
