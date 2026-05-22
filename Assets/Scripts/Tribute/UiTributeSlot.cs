using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTributeSlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image iconImage;
    public Image imageCompleted;
    public Button button;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    private ItemAsset itemAsset;
    public ItemAsset ItemAsset => itemAsset;
    private int required;
    private int submitted;
    public int Submitted => submitted;

    public string ItemID => itemAsset.ItemID;
    public int Required => required;
    public bool IsComplete => submitted == required;

    public void SetSlot(ItemAsset asset, int requiredAmount)
    {
        itemAsset = asset;
        if (itemAsset.Data == null)
            itemAsset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(asset.ItemID);
        required = requiredAmount;
        submitted = 0;
        UpdateDisplay();
    }

    public void AddSubmit(int amount)
    {
        submitted += amount;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        iconImage.sprite = itemAsset.Icon;
        nameText.text = itemAsset.Data.DisplayName;
        amountText.text = $"{submitted} / {required}";

        imageCompleted.gameObject.SetActive(IsComplete);
        button.interactable = !IsComplete;
    }
}
