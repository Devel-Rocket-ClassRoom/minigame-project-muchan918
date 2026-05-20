using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemInfo : MonoBehaviour
{
    public Image imageIcon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textType;

    public void SetEmpty()
    {
        gameObject.SetActive(false);
        imageIcon.sprite = null;
        textName.text = string.Empty;
        textType.text = string.Empty;
    }

    public void SetItem(ItemAsset asset)
    {
        gameObject.SetActive(true);
        imageIcon.sprite = asset.Icon;
        textName.text = asset.Data.DisplayName;
        textType.text = asset.Data.ItemType;
    }
}
