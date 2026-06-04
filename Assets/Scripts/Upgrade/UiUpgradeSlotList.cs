using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiUpgradeSlotList : MonoBehaviour
{
    public UiUpgradeSlot prefab;
    public ScrollRect scrollRect;
    public UpgradeInteraction upgradeInteraction;

    private List<UiUpgradeSlot> slotList = new List<UiUpgradeSlot>();
    private List<UpgradeAsset> assetList = new List<UpgradeAsset>();

    public void Setup(List<UpgradeAsset> assets)
    {
        foreach (var asset in assets)
            AddSlot(asset);
    }

    private void AddSlot(UpgradeAsset asset)
    {
        assetList.Add(asset);

        var slot = Instantiate(prefab, scrollRect.content);
        slot.slotIndex = slotList.Count;
        slot.SetSlot(asset);

        int capturedIndex = slotList.Count;
        slot.button.onClick.AddListener(() =>
        {
            upgradeInteraction.OnSelectUpgrade(assetList[capturedIndex]);
        });

        slotList.Add(slot);
    }

    public void UpdateAllLevels()
    {
        foreach (var slot in slotList)
            slot.UpdateLevel();
    }
}
