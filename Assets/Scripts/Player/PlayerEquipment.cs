using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    private Transform _partsRoot;
    private readonly Dictionary<EquipSlotType, (string partsName, int index)> _equipped = new();

    // 슬롯별 디폴트 파츠 (장비 착용 시 비활성화, 해제 시 활성화)
    private readonly Dictionary<EquipSlotType, (string partsName, int index)> _defaults = new()
    {
        { EquipSlotType.Top, ("Top", 1) }, // Top_02 (인덱스 1)
        { EquipSlotType.Bottom, ("Bottom", 1) }, // Bottom_02 (인덱스 1)
    };

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

        // 디폴트 파츠 비활성화
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
        _equipped[data.SlotType] = (data.PartsName, index);
    }

    public void UnEquip(EquipSlotType slot)
    {
        if (!_equipped.TryGetValue(slot, out var current))
        {
            // 장착된 게 없어도 디폴트는 비활성화 해제
            SetDefaultActive(slot, true);
            return;
        }

        Transform partsObj = _partsRoot.Find(current.partsName);
        if (partsObj != null)
            partsObj.GetChild(current.index).gameObject.SetActive(false);

        _equipped.Remove(slot);

        // 디폴트 파츠 활성화
        SetDefaultActive(slot, true);
    }

    public void UnEquipAll()
    {
        foreach (var slot in _equipped.Keys.ToList())
            UnEquip(slot);
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
