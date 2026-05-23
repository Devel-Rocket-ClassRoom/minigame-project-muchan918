using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiIngredientSlot : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    private int required;
    private int owned;

    public bool IsFulfilled => owned >= required;

    public void SetSlot(ItemAsset asset, int requiredAmount, int ownedAmount)
    {
        if (asset.Data == null)
            asset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(asset.ItemID);

        required = requiredAmount;
        owned = ownedAmount;

        iconImage.sprite = asset.Icon;
        nameText.text = asset.Data.DisplayName;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        amountText.text = $"{required}개 필요 (보유 {owned})";
        amountText.color = IsFulfilled ? Color.white : Color.red;
    }
}
