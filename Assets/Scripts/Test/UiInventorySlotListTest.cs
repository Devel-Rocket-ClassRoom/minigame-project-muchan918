using UnityEngine;
using UnityEngine.InputSystem;

public class UiInventorySlotListTest : MonoBehaviour
{
    public UiInventorySlotList slotList;
    public ItemAsset[] testAssets;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            slotList.AddItem(testAssets[Random.Range(0, testAssets.Length)]);

        if (Keyboard.current.rKey.wasPressedThisFrame)
            slotList.RemoveItem();

        if (Keyboard.current.cKey.wasPressedThisFrame)
            slotList.Clear();
    }
}
