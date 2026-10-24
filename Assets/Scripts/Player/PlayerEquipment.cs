using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    [SerializeField]
    private PreviewEquipment previewEquipment;

    private Transform partsRoot;
    private readonly Dictionary<
        EquipSlotType,
        (string partsName, int index, ItemAsset item, int value, EquipmentData data)
    > equipped = new();

    private readonly Dictionary<EquipSlotType, (string partsName, int index)> defaults = new()
    {
        { EquipSlotType.Top, ("Top", 1) },
        { EquipSlotType.Bottom, ("Bottom", 1) },
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        partsRoot = transform.Find("Parts");
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

        Transform partsObj = partsRoot.Find(data.PartsName);
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
        equipped[data.SlotType] = (data.PartsName, index, item, data.Value, data);

        ApplyValue(data.SlotType, data.Value);

        previewEquipment?.Equip(data); // 동기화
    }

    public ItemAsset UnEquip(EquipSlotType slot)
    {
        if (!equipped.TryGetValue(slot, out var current))
        {
            SetDefaultActive(slot, true);
            return null;
        }

        Transform partsObj = partsRoot.Find(current.partsName);
        if (partsObj != null)
            partsObj.GetChild(current.index).gameObject.SetActive(false);

        equipped.Remove(slot);
        SetDefaultActive(slot, true);

        ApplyValue(slot, -current.value);

        previewEquipment?.UnEquip(slot); // 동기화

        return current.item;
    }

    public void UnEquipAll()
    {
        foreach (var slot in equipped.Keys.ToList())
            UnEquip(slot);
    }

    public ItemAsset GetEquippedItem(EquipSlotType slot)
    {
        equipped.TryGetValue(slot, out var current);
        return current.item;
    }

    public bool IsEquipped(EquipSlotType slot) => equipped.ContainsKey(slot);

    public EquipmentData GetWeaponRightData()
    {
        if (!equipped.TryGetValue(EquipSlotType.WeaponRight, out var current))
            return null;

        return current.data;
    }

    private void ApplyValue(EquipSlotType slot, int value)
    {
        if (slot == EquipSlotType.WeaponRight)
            PlayerAction.Instance.AddDamage(Mathf.RoundToInt(value / 3f));
        else
            PlayerHealth.Instance.AddMaxHp(value);
    }

    private void SetDefaultActive(EquipSlotType slot, bool active)
    {
        if (!defaults.TryGetValue(slot, out var def))
            return;

        Transform partsObj = partsRoot.Find(def.partsName);
        if (partsObj != null && def.index < partsObj.childCount)
            partsObj.GetChild(def.index).gameObject.SetActive(active);
    }

    public bool IsAllEquipped()
    {
        return equipped.ContainsKey(EquipSlotType.WeaponRight)
            && equipped.ContainsKey(EquipSlotType.Hat)
            && equipped.ContainsKey(EquipSlotType.Top)
            && equipped.ContainsKey(EquipSlotType.Bottom)
            && equipped.ContainsKey(EquipSlotType.Shoes);
    }
}
