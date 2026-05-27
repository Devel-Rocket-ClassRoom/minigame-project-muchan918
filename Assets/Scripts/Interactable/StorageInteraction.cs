using System.Collections;
using TMPro;
using UnityEngine;

public class StorageInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Storage;

    [Header("Data")]
    [SerializeField]
    private PlayerInventory playerInventory;

    [Header("UI")]
    [SerializeField]
    private GameObject storagePanel;

    [SerializeField]
    private GameObject buttonsPanel;

    [Header("Full Popup")]
    [SerializeField]
    private GameObject fullPopup;

    [SerializeField]
    private TextMeshProUGUI fullPopupText;

    [SerializeField]
    private float fullPopupDuration = 1f;

    private Coroutine fullPopupCoroutine;

    [Header("Components")]
    [SerializeField]
    private StorageInventory storageInventory;

    [SerializeField]
    private StoragePlayerInventory playerInventoryMirror;

    private ItemAsset selectedAsset;
    private int selectedAmount;
    private bool isStorageSide;

    private void Awake()
    {
        storagePanel.SetActive(false);
        buttonsPanel.SetActive(false);
        fullPopup.SetActive(false);

        storageInventory.OnSlotClicked += (previousIndex) =>
        {
            if (isStorageSide && previousIndex == storageInventory.SelectedSlotIndex)
            {
                selectedAsset = storageInventory.GetSelectedAsset();
                selectedAmount = storageInventory.GetSelectedAmount();
                MoveItem(true, 1);
                return;
            }

            playerInventoryMirror.ResetSelection();
            selectedAsset = storageInventory.GetSelectedAsset();
            selectedAmount = storageInventory.GetSelectedAmount();
            isStorageSide = true;
            buttonsPanel.SetActive(true);
        };

        playerInventoryMirror.OnSlotClicked += (previousIndex) =>
        {
            if (!isStorageSide && previousIndex == playerInventoryMirror.SelectedSlotIndex)
            {
                selectedAsset = playerInventoryMirror.GetSelectedAsset();
                selectedAmount = playerInventoryMirror.GetSelectedAmount();
                MoveItem(false, 1);
                return;
            }

            storageInventory.ResetSelection();
            selectedAsset = playerInventoryMirror.GetSelectedAsset();
            selectedAmount = playerInventoryMirror.GetSelectedAmount();
            isStorageSide = false;
            buttonsPanel.SetActive(true);
        };
    }

    public void Interact(GameObject player)
    {
        storagePanel.SetActive(true);
        storageInventory.UpdateSlots();
        playerInventoryMirror.UpdateSlots();
        buttonsPanel.SetActive(false);
        GamePause.Pause();
    }

    public void OnClickAll()
    {
        MoveItem(isStorageSide, selectedAmount);
    }

    public void OnClickHalf()
    {
        int halfAmount = Mathf.Max(1, Mathf.FloorToInt(selectedAmount / 2f));
        MoveItem(isStorageSide, halfAmount);
    }

    public void OnClickClose()
    {
        ResetSelection();
        storagePanel.SetActive(false);
        GamePause.Resume();
    }

    private void MoveItem(bool fromStorage, int amount)
    {
        if (selectedAsset == null)
            return;

        if (fromStorage)
        {
            int moved = playerInventory.SlotList.AddItem(selectedAsset, amount);
            if (moved > 0)
            {
                storageInventory.RemoveFromSelected(moved);
                playerInventoryMirror.UpdateSlots();
            }
            if (moved < amount)
                ShowFullPopup("인벤토리가 가득 찼습니다.");
        }
        else
        {
            int moved = storageInventory.AddItem(selectedAsset, amount);
            if (moved > 0)
            {
                playerInventoryMirror.RemoveFromSelected(moved);
                playerInventoryMirror.UpdateSlots();
            }
            if (moved < amount)
                ShowFullPopup("창고가 가득 찼습니다.");
        }

        playerInventory.SlotList.UpdateSlots();
        ResetSelection();
    }

    private void ShowFullPopup(string message)
    {
        fullPopupText.text = message;

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

    private void ResetSelection()
    {
        selectedAsset = null;
        selectedAmount = 0;
        storageInventory.ResetSelection();
        playerInventoryMirror.ResetSelection();
        buttonsPanel.SetActive(false);
    }
}
