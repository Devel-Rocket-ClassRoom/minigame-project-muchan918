using UnityEngine;
using UnityEngine.InputSystem;

public class StorageTest : MonoBehaviour
{
    public StoragePlayerInventory playerInventoryMirror;
    public GameObject storagePanel;

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            storagePanel.SetActive(true);
            playerInventoryMirror.UpdateSlots();
        }
    }

    public void TestClose()
    {
        storagePanel.SetActive(false);
    }
}
