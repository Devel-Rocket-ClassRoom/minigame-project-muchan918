using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private UiInventorySlotList slotList;
    public UiInventorySlotList SlotList => slotList;

    [Header("Capacity")]
    [SerializeField]
    private GameObject fullPopup;

    [SerializeField]
    private float fullPopupDuration = 1f;

    private Coroutine fullPopupCoroutine;
    private bool isOpen;

    private void Awake()
    {
        inventoryPanel.SetActive(false);
        fullPopup.SetActive(false);
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

        if (isOpen)
        {
            slotList.UpdateSlots();
        }
    }

    public bool AddItem(ItemAsset asset) // void → bool 반환
    {
        bool success = slotList.AddItem(asset);
        if (!success)
            ShowFullPopup();
        return success; // 결과 반환
    }

    private void ShowFullPopup()
    {
        if (fullPopup == null)
            return;

        if (fullPopupCoroutine != null)
            StopCoroutine(fullPopupCoroutine);

        fullPopupCoroutine = StartCoroutine(FullPopupRoutine());
    }

    private IEnumerator FullPopupRoutine()
    {
        fullPopup.SetActive(true);
        yield return new WaitForSecondsRealtime(fullPopupDuration);
        fullPopup.SetActive(false);
        fullPopupCoroutine = null;
    }
}
