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
    public bool IsOpen => isOpen;

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
            UiManager.Instance.Register(Close); // 열릴 때 등록
        }
        else
        {
            UiManager.Instance.Unregister(Close); // 닫힐 때 해제
        }
    }

    private void Close()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
    }

    public bool AddItem(ItemAsset asset) // void → bool 반환
    {
        bool success = slotList.AddItem(asset);
        if (!success)
            ShowFullPopup();
        return success; // 결과 반환
    }

    public int AddItem(ItemAsset asset, int amount)
    {
        int moved = slotList.AddItem(asset, amount);
        if (moved < amount)
            ShowFullPopup();
        return moved;
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
