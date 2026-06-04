using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiUpgradeSlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image iconImage;
    public Button button;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    private UpgradeAsset asset;
    public UpgradeAsset Asset => asset;

    public void SetSlot(UpgradeAsset upgradeAsset)
    {
        asset = upgradeAsset;
        iconImage.sprite = asset.icon;
        nameText.text = asset.displayName;
        UpdateLevel();
        gameObject.SetActive(true);
    }

    public void UpdateLevel()
    {
        int current = UpgradeManager.Instance.GetLevel(asset.type);
        levelText.text = $"Lv.{current} / {asset.MaxLevel}";
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }
}
