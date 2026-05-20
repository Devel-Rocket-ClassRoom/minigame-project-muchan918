using UnityEngine;
using UnityEngine.UI;

public class UiInventorySlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image imageIcon;
    public Button button;

    public ItemAsset itemAsset { get; private set; }

    public void SetItem(ItemAsset asset)
    {
        itemAsset = asset;
        imageIcon.sprite = asset.Icon;
        gameObject.SetActive(true);
    }

    public void SetEmpty()
    {
        itemAsset = null;
        imageIcon.sprite = null;
        gameObject.SetActive(false);
    }
}
