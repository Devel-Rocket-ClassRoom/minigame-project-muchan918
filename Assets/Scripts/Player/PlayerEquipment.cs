using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    private Transform _partsRoot;
    private readonly Dictionary<
        EquipSlotType,
        (string partsName, int index, ItemAsset item)
    > _equipped = new();

    private readonly Dictionary<EquipSlotType, (string partsName, int index)> _defaults = new()
    {
        { EquipSlotType.Top, ("Top", 1) },
        { EquipSlotType.Bottom, ("Bottom", 1) },
    };

    private void Awake()
    {
        Instance = this;
        _partsRoot = transform.Find("Parts");
    }

    public void Equip(EquipmentAsset asset, ItemAsset item)
    {
        if (asset.Data == null)
            asset.Data = DataTableManager
                .Get<EquipmentTable>("EquipmentTable")
                .Get(asset.EquipmentID);

        Equip(asset.Data, item);
    }

    public void Equip(EquipmentData data, ItemAsset item)
    {
        UnEquip(data.SlotType);
        SetDefaultActive(data.SlotType, false);

        Transform partsObj = _partsRoot.Find(data.PartsName);
        if (partsObj == null)
        {
            Debug.LogWarning($"Parts에서 {data.PartsName} 못 찾음");
            return;
        }

        int index = data.PartsIndex;
        if (index >= partsObj.childCount)
        {
            Debug.LogWarning($"{data.PartsName}에 인덱스 {index} 없음");
            return;
        }

        partsObj.GetChild(index).gameObject.SetActive(true);
        _equipped[data.SlotType] = (data.PartsName, index, item);
    }

    public ItemAsset UnEquip(EquipSlotType slot)
    {
        if (!_equipped.TryGetValue(slot, out var current))
        {
            SetDefaultActive(slot, true);
            return null;
        }

        Transform partsObj = _partsRoot.Find(current.partsName);
        if (partsObj != null)
            partsObj.GetChild(current.index).gameObject.SetActive(false);

        _equipped.Remove(slot);
        SetDefaultActive(slot, true);

        return current.item;
    }

    public void UnEquipAll()
    {
        foreach (var slot in _equipped.Keys.ToList())
            UnEquip(slot);
    }

    public ItemAsset GetEquippedItem(EquipSlotType slot)
    {
        _equipped.TryGetValue(slot, out var current);
        return current.item;
    }

    public bool IsEquipped(EquipSlotType slot) => _equipped.ContainsKey(slot);

    private void SetDefaultActive(EquipSlotType slot, bool active)
    {
        if (!_defaults.TryGetValue(slot, out var def))
            return;

        Transform partsObj = _partsRoot.Find(def.partsName);
        if (partsObj != null && def.index < partsObj.childCount)
            partsObj.GetChild(def.index).gameObject.SetActive(active);
    }
}
