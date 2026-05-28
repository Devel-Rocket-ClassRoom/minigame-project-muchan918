using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

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

        // 같은 슬롯 기존 장비 해제
        UnEquip(asset.Data.SlotType);

        // PartsName으로 오브젝트 찾고 PartsIndex로 활성화
        Transform partsObj = _partsRoot.Find(asset.Data.PartsName);
        if (partsObj == null)
        {
            Debug.LogWarning($"Parts에서 {asset.Data.PartsName} 못 찾음");
            return;
        }

        int index = asset.Data.PartsIndex;
        if (index >= partsObj.childCount)
        {
            Debug.LogWarning($"{asset.Data.PartsName}에 인덱스 {index} 없음");
            return;
        }

        partsObj.GetChild(index).gameObject.SetActive(true);
        _equipped[asset.Data.SlotType] = (asset.Data.PartsName, index);
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
