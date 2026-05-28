using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    private Transform _partsRoot;
    private readonly Dictionary<EquipSlotType, (string partsName, int index)> _equipped = new();

    private void Awake()
    {
        Instance = this;
        _partsRoot = transform.Find("Parts");
    }

    public void Equip(EquipmentAsset asset)
    {
        if (asset.Data == null)
            asset.Data = DataTableManager
                .Get<EquipmentTable>("EquipmentTable")
                .Get(asset.EquipmentID);

        Equip(asset.Data);
    }

    public void Equip(EquipmentData data)
    {
        UnEquip(data.SlotType);

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
        _equipped[data.SlotType] = (data.PartsName, index);
    }

    public void UnEquip(EquipSlotType slot)
    {
        if (!_equipped.TryGetValue(slot, out var current))
            return;

        Transform partsObj = _partsRoot.Find(current.partsName);
        if (partsObj != null)
            partsObj.GetChild(current.index).gameObject.SetActive(false);

        _equipped.Remove(slot);
    }

    public void UnEquipAll()
    {
        foreach (var slot in _equipped.Keys.ToList())
            UnEquip(slot);
    }
}
