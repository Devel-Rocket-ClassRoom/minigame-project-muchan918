using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentTest : MonoBehaviour
{
    [Header("테스트할 장비 SO")]
    public EquipmentAsset hat;
    public EquipmentAsset top;
    public EquipmentAsset bottom;
    public EquipmentAsset shoes;
    public EquipmentAsset weaponRight;
    public EquipmentAsset weaponBack;
    public EquipmentAsset hat2;

    private void Update()
    {
        if (EquipmentManager.Instance == null)
        {
            Debug.LogError("EquipmentManager.Instance가 null!");
            return;
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            EquipmentManager.Instance.Equip(hat);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            EquipmentManager.Instance.Equip(top);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            EquipmentManager.Instance.Equip(bottom);

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            EquipmentManager.Instance.Equip(shoes);

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
            EquipmentManager.Instance.Equip(weaponRight);

        if (Keyboard.current.digit6Key.wasPressedThisFrame)
            EquipmentManager.Instance.Equip(weaponBack);

        if (Keyboard.current.digit7Key.wasPressedThisFrame)
            EquipmentManager.Instance.Equip(hat2);

        if (Keyboard.current.qKey.wasPressedThisFrame)
            EquipmentManager.Instance.UnEquipAll();
    }
}
