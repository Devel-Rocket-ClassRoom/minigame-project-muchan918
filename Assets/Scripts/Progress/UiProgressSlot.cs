using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiProgressSlot : MonoBehaviour
{
    public Image iconImage;
    public Image imageCompleted;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    public void SetSlot(ItemAsset item, int submitted, int required, bool isComplete)
    {
        if (item.Data == null)
            item.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(item.ItemID);

        iconImage.sprite = item.Icon;
        nameText.text = item.Data.DisplayName;
        amountText.text = $"{submitted} / {required}";
        imageCompleted.gameObject.SetActive(isComplete);
    }
}
