// PreviewEquipment.cs - PreviewPlayer에 붙이는 새 컴포넌트
using System.Collections.Generic;
using UnityEngine;

public class PreviewEquipment : MonoBehaviour
{
    private Transform partsRoot;
    private readonly Dictionary<EquipSlotType, (string partsName, int index)> equipped = new();
    private readonly Dictionary<EquipSlotType, (string partsName, int index)> defaults = new()
    {
        { EquipSlotType.Top, ("Top", 1) },
        { EquipSlotType.Bottom, ("Bottom", 1) },
    };

    private void Awake()
    {
        partsRoot = transform.Find("Parts");
    }

    public void Equip(EquipmentData data)
    {
        UnEquip(data.SlotType);
        SetDefaultActive(data.SlotType, false);

        Transform partsObj = partsRoot.Find(data.PartsName);
        if (partsObj == null)
            return;
        if (data.PartsIndex >= partsObj.childCount)
            return;

        partsObj.GetChild(data.PartsIndex).gameObject.SetActive(true);
        equipped[data.SlotType] = (data.PartsName, data.PartsIndex);
    }

    public void UnEquip(EquipSlotType slot)
    {
        if (!equipped.TryGetValue(slot, out var current))
        {
            SetDefaultActive(slot, true);
            return;
        }

        Transform partsObj = partsRoot.Find(current.partsName);
        if (partsObj != null)
            partsObj.GetChild(current.index).gameObject.SetActive(false);

        equipped.Remove(slot);
        SetDefaultActive(slot, true);
    }

    private void SetDefaultActive(EquipSlotType slot, bool active)
    {
        if (!defaults.TryGetValue(slot, out var def))
            return;
        Transform partsObj = partsRoot.Find(def.partsName);
        if (partsObj != null && def.index < partsObj.childCount)
            partsObj.GetChild(def.index).gameObject.SetActive(active);
    }
}
