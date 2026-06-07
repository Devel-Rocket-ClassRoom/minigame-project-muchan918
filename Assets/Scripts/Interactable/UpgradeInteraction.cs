using System.Collections.Generic;
using UnityEngine;

public class UpgradeInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Upgrade;

    [Header("Data")]
    [SerializeField]
    private List<UpgradeAsset> upgradeList;

    [Header("UI")]
    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private GameObject ingredientPanel;

    [SerializeField]
    private UiUpgradeSlotList upgradeSlotList;

    [SerializeField]
    private UiUpgradeIngredientSlotList ingredientSlotList;

    [SerializeField]
    private UnityEngine.UI.Button upgradeButton;

    private UpgradeAsset selectedAsset;

    private void Awake()
    {
        upgradePanel.SetActive(false);
        ingredientPanel.SetActive(false);
    }

    private void Start()
    {
        upgradeSlotList.Setup(upgradeList);
    }

    public void Interact(GameObject player)
    {
        upgradePanel.SetActive(true);
        UiManager.Instance.Register(OnClickClose);

        if (TutorialSceneManager.Instance != null)
            TutorialSceneManager.Instance.CompleteStep(TutorialStep.GoToUpgrade);
    }

    public void OnSelectUpgrade(UpgradeAsset asset)
    {
        selectedAsset = asset;

        int level = UpgradeManager.Instance.GetLevel(asset.type);
        ingredientSlotList.Setup(asset, level);
        upgradeButton.interactable = UpgradeManager.Instance.CanAfford(asset);
        ingredientPanel.SetActive(true);
    }

    public void OnClickUpgrade()
    {
        if (selectedAsset == null)
            return;

        bool success = UpgradeManager.Instance.Upgrade(selectedAsset);
        if (!success)
            return;

        upgradeSlotList.UpdateAllLevels();
        OnSelectUpgrade(selectedAsset);

        if (TutorialSceneManager.Instance != null)
            TutorialSceneManager.Instance.CompleteStep(TutorialStep.Upgrade);
    }

    public void OnClickClose()
    {
        UiManager.Instance.Unregister(OnClickClose);
        ingredientPanel.SetActive(false);
        upgradePanel.SetActive(false);
        selectedAsset = null;
    }
}
