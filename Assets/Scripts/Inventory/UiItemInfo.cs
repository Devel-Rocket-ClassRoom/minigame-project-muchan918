using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiItemInfo : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    public Image imageIcon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textType;
    public Button equipButton;
    public Button unequipButton;
    public Button eatButton;
    public Button dropButton;
    public Button endingButton;

    public UiInventorySlotList inventorySlotList;
    public UiEquipPanel equipPanel;

    private void Awake()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
        imageIcon.sprite = null;
        textName.text = string.Empty;
        textType.text = string.Empty;
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
        eatButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
        endingButton.gameObject.SetActive(false);
    }

    public void SetItem(ItemAsset asset)
    {
        gameObject.SetActive(true);
        imageIcon.sprite = asset.Icon;
        textName.text = asset.Data.DisplayName;
        textType.text = asset.Data.ItemType;

        bool isEquipment = asset.Data.ItemType == "Equipment";
        bool isFood = asset.Data.ItemType == "Food";
        bool isEnding = asset.Data.ItemType == "Ending";

        equipButton.gameObject.SetActive(isEquipment);
        unequipButton.gameObject.SetActive(false);
        eatButton.gameObject.SetActive(isFood);
        dropButton.gameObject.SetActive(true);
        endingButton.gameObject.SetActive(isEnding);

        if (isEquipment)
        {
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() =>
            {
                var equipData = DataTableManager
                    .Get<EquipmentTable>("EquipmentTable")
                    .Get(asset.ItemID);

                // 기존 장착 아이템 있으면 인벤토리에 돌려주기
                ItemAsset previousItem = PlayerEquipment.Instance.GetEquippedItem(
                    equipData.SlotType
                );
                if (previousItem != null)
                {
                    if (inventorySlotList.IsFull)
                    {
                        Debug.Log("인벤토리가 꽉 찼습니다!");
                        // TODO: 팝업
                        return;
                    }
                    inventorySlotList.AddItem(previousItem);
                }

                PlayerEquipment.Instance.Equip(equipData, asset);
                equipPanel.Equip(equipData.SlotType, asset);
                inventorySlotList.RemoveItemByAsset(asset, 1);
                SetEmpty();
            });
        }

        if (isFood)
        {
            eatButton.onClick.RemoveAllListeners();
            eatButton.onClick.AddListener(() =>
            {
                var foodData = DataTableManager.Get<FoodTable>("FoodTable").Get(asset.ItemID);

                if (foodData.EffectType == FoodEffectType.Hunger)
                    PlayerHunger.Instance.AddHunger(foodData.Value);
                else if (foodData.EffectType == FoodEffectType.Hp)
                    PlayerHealth.Instance.Recover(foodData.Value);

                inventorySlotList.RemoveItem(1);
                SetEmpty();
            });
        }
        if (isEnding)
        {
            endingButton.onClick.RemoveAllListeners();
            endingButton.onClick.AddListener(() =>
            {
                FadeOutAndLoadEnding();
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
        dropButton.gameObject.SetActive(false);

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

    private void FadeOutAndLoadEnding()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
        fadeImage
            .DOFade(1f, 1f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(
                    0.5f,
                    () =>
                    {
                        SceneManager.LoadScene("Ending");
                    },
                    true
                );
            });
    }
}
