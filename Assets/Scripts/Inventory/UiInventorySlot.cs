using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventorySlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image imageIcon;
    public Button button;
    public TextMeshProUGUI amountText;

    public ItemAsset itemAsset { get; private set; }

    public void SetItem(ItemAsset asset)
    {
        itemAsset = asset;
        imageIcon.sprite = asset.Icon;
        gameObject.SetActive(true);
    }

    public void SetItem(ItemAsset asset, int amount)
    {
        itemAsset = asset;
        imageIcon.sprite = asset.Icon;
        amountText.text = amount.ToString();
        gameObject.SetActive(true);
    }

    public void SetEmpty()
    {
        itemAsset = null;
        imageIcon.sprite = null;
        amountText.text = "";
        gameObject.SetActive(false);
    }
}
