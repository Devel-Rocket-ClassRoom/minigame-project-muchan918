using EquipSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipTest : MonoBehaviour
{
    [Header("테스트할 프리팹들")]
    public GameObject hatPrefab;
    public GameObject topPrefab;
    public GameObject bottomPrefab;
    public GameObject shoesPrefab;
    public GameObject weaponRightPrefab;
    public GameObject weaponBackPrefab;

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            EquipManager.Instance.Equip(EquipSlotType.Hat, hatPrefab);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            EquipManager.Instance.Equip(EquipSlotType.Top, topPrefab);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            EquipManager.Instance.Equip(EquipSlotType.Bottom, bottomPrefab);

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            EquipManager.Instance.Equip(EquipSlotType.Shoes, shoesPrefab);

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
            EquipManager.Instance.Equip(EquipSlotType.WeaponRight, weaponRightPrefab);

        if (Keyboard.current.digit6Key.wasPressedThisFrame)
            EquipManager.Instance.Equip(EquipSlotType.WeaponBack, weaponBackPrefab);

        if (Keyboard.current.qKey.wasPressedThisFrame)
            EquipManager.Instance.UnEquipAll();
    }
}
