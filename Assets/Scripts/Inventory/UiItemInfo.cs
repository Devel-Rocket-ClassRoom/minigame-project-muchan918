using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemInfo : MonoBehaviour
{
    public Image imageIcon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textType;
    public Button equipButton;
    public Button unequipButton;

    public UiInventorySlotList inventorySlotList;
    public UiEquipPanel equipPanel;

    public void SetEmpty()
    {
        gameObject.SetActive(false);
        imageIcon.sprite = null;
        textName.text = string.Empty;
        textType.text = string.Empty;
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
    }

    public void SetItem(ItemAsset asset)
    {
        gameObject.SetActive(true);
        imageIcon.sprite = asset.Icon;
        textName.text = asset.Data.DisplayName;
        textType.text = asset.Data.ItemType;

        bool isEquipment = asset.Data.ItemType == "Equipment";
        equipButton.gameObject.SetActive(isEquipment);
        unequipButton.gameObject.SetActive(false);

        if (isEquipment)
        {
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() =>
            {
                var equipData = DataTableManager
                    .Get<EquipmentTable>("EquipmentTable")
                    .Get(asset.ItemID);

                PlayerEquipment.Instance.Equip(equipData, asset);
                equipPanel.Equip(equipData.SlotType, asset);
                inventorySlotList.RemoveItemByAsset(asset, 1);
                SetEmpty();
            });
        }
    }

    public void SetEquippedItem(ItemAsset asset, EquipSlotType slotType)
    {
        gameObject.SetActive(true);
        imageIcon.sprite = asset.Icon;
        textName.text = asset.Data.DisplayName;
        textType.text = asset.Data.ItemType;

        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(true);

        unequipButton.onClick.RemoveAllListeners();
        unequipButton.onClick.AddListener(() =>
        {
            if (inventorySlotList.IsFull)
            {
                Debug.Log("인벤토리가 꽉 찼습니다!");
                // TODO: 꽉 찼다는 팝업 표시
                return;
            }

            ItemAsset unequipped = PlayerEquipment.Instance.UnEquip(slotType);
            if (unequipped != null)
            {
                inventorySlotList.AddItem(unequipped);
                equipPanel.UnEquip(slotType);
            }

            SetEmpty();
        });
    }
}
