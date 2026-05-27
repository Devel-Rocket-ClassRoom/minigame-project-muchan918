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
            GamePause.Pause();
        }
        else
        {
            GamePause.Resume();
        }
    }

    public void AddItem(ItemAsset asset)
    {
        bool success = slotList.AddItem(asset);
        if (!success)
            ShowFullPopup();
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
