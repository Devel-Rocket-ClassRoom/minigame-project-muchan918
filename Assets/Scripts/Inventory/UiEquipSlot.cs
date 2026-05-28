using UnityEngine;
using UnityEngine.UI;

public class UiEquipSlot : MonoBehaviour
{
    public EquipSlotType slotType;
    public GameObject item;
    public Image imageIcon;
    public Button button;

    private ItemAsset _equippedItem;

    private void Awake()
    {
        item.SetActive(false);
    }

    public void SetItem(ItemAsset asset)
    {
        _equippedItem = asset;
        imageIcon.sprite = asset.Icon;
        item.SetActive(true);
    }

    public void SetEmpty()
    {
        _equippedItem = null;
        imageIcon.sprite = null;
        item.SetActive(false);
    }
}
