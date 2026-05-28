using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemInfo : MonoBehaviour
{
    public Image imageIcon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textType;
    public Button equipButton;

    public void SetEmpty()
    {
        gameObject.SetActive(false);
        imageIcon.sprite = null;
        textName.text = string.Empty;
        textType.text = string.Empty;
        equipButton.gameObject.SetActive(false);
    }

    public void SetItem(ItemAsset asset)
    {
        gameObject.SetActive(true);
        imageIcon.sprite = asset.Icon;
        textName.text = asset.Data.DisplayName;
        textType.text = asset.Data.ItemType;

        bool isEquipment = asset.Data.ItemType == "Equipment";
        equipButton.gameObject.SetActive(isEquipment);

        if (isEquipment)
        {
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() =>
            {
                var equipData = DataTableManager
                    .Get<EquipmentTable>("EquipmentTable")
                    .Get(asset.ItemID);
                PlayerEquipment.Instance.Equip(equipData);
            });
        }
    }
}
