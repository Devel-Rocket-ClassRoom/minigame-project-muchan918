// PreviewEquipment.cs - PreviewPlayer에 붙이는 새 컴포넌트
using System.Collections.Generic;
using UnityEngine;

public class PreviewEquipment : MonoBehaviour
{
    private Transform _partsRoot;
    private readonly Dictionary<EquipSlotType, (string partsName, int index)> _equipped = new();
    private readonly Dictionary<EquipSlotType, (string partsName, int index)> _defaults = new()
    {
        { EquipSlotType.Top, ("Top", 1) },
        { EquipSlotType.Bottom, ("Bottom", 1) },
    };

    private void Awake()
    {
        _partsRoot = transform.Find("Parts");
    }

    public void Equip(EquipmentData data)
    {
        UnEquip(data.SlotType);
        SetDefaultActive(data.SlotType, false);

        Transform partsObj = _partsRoot.Find(data.PartsName);
        if (partsObj == null)
            return;
        if (data.PartsIndex >= partsObj.childCount)
            return;

        partsObj.GetChild(data.PartsIndex).gameObject.SetActive(true);
        _equipped[data.SlotType] = (data.PartsName, data.PartsIndex);
    }

    public void UnEquip(EquipSlotType slot)
    {
        if (!_equipped.TryGetValue(slot, out var current))
        {
            SetDefaultActive(slot, true);
            return;
        }

        Transform partsObj = _partsRoot.Find(current.partsName);
        if (partsObj != null)
            partsObj.GetChild(current.index).gameObject.SetActive(false);

        _equipped.Remove(slot);
        SetDefaultActive(slot, true);
    }

    private void SetDefaultActive(EquipSlotType slot, bool active)
    {
        if (!_defaults.TryGetValue(slot, out var def))
            return;
        Transform partsObj = _partsRoot.Find(def.partsName);
        if (partsObj != null && def.index < partsObj.childCount)
            partsObj.GetChild(def.index).gameObject.SetActive(active);
    }
}
