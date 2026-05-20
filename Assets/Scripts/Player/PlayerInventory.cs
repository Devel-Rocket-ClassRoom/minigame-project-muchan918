using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private UiInventorySlotList slotList;

    private bool isOpen;

    private void Awake()
    {
        inventoryPanel.SetActive(false);
        isOpen = false;
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
            Toggle();
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        GamePause.Toggle();
    }

    public void AddItem(ItemAsset asset)
    {
        slotList.AddItem(asset);
    }
}
