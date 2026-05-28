using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiUpgradeSlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image iconImage;
    public Image outlineImage;
    public Button button;
    public TextMeshProUGUI nameText;

    private UpgradeAsset asset;
    public UpgradeAsset Asset => asset;

    public void SetSlot(UpgradeAsset upgradeAsset)
    {
        asset = upgradeAsset;
        iconImage.sprite = asset.icon;
        nameText.text = asset.displayName;
        gameObject.SetActive(true);
    }

    public void SetSelected(bool selected)
    {
        outlineImage.gameObject.SetActive(selected);
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }
}
