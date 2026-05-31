using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventorySlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image imageIcon;
    public Image imageBg;
    public Image imageOutlineSelected;
    public Button button;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI nameText;

    public ItemAsset itemAsset { get; private set; }

    public void SetItem(ItemAsset asset)
    {
        itemAsset = asset;
        imageIcon.sprite = asset.Icon;
        nameText.text = asset.Data.DisplayName;
        gameObject.SetActive(true);
    }

    public void SetItem(ItemAsset asset, int amount)
    {
        itemAsset = asset;
        imageIcon.sprite = asset.Icon;
        amountText.text = amount.ToString();
        nameText.text = asset.Data.DisplayName;
        gameObject.SetActive(true);
    }

    public void SetEmpty()
    {
        itemAsset = null;
        imageIcon.sprite = null;
        amountText.text = "";
        nameText.text = "";
        gameObject.SetActive(false);
    }

    public void SetGray()
    {
        imageIcon.color = Color.gray;
        imageBg.color = Color.gray;
        button.interactable = false;
        imageOutlineSelected.gameObject.SetActive(false);
    }

    public void SetNormal()
    {
        imageIcon.color = Color.white;
        imageBg.color = Color.white;
        button.interactable = true;
        imageOutlineSelected.gameObject.SetActive(false);
    }

    public void SetSelected()
    {
        imageOutlineSelected.gameObject.SetActive(true);
    }
}
