using UnityEngine;
using UnityEngine.UI;

public class UiEquipSlot : MonoBehaviour
{
    public EquipSlotType slotType;
    public GameObject item;
    public Image imageIcon;
    public Button button;

    private ItemAsset equippedItem;

    public void SetItem(ItemAsset asset)
    {
        equippedItem = asset;
        imageIcon.sprite = asset.Icon;
        item.SetActive(true);
    }

    public void SetEmpty()
    {
        equippedItem = null;
        imageIcon.sprite = null;
        item.SetActive(false);
    }
}
