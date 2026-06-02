using UnityEngine;

public class AltarInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Altar;

    [SerializeField]
    private TributeEvent tributeEvent;

    [SerializeField]
    private TributeInventory tributeInventory;

    [SerializeField]
    private UiTributeSlotList tributeSlotList;

    [SerializeField]
    private UiSubmitPanel submitPanel;

    [Header("UI")]
    [SerializeField]
    private GameObject altarPanel;

    private void Awake()
    {
        altarPanel.SetActive(false);
    }

    public void Interact(GameObject player)
    {
        altarPanel.SetActive(true);
        tributeInventory.UpdateSlots();
        UiManager.Instance.Register(OnClickClose);
    }

    public void OnClickClose()
    {
        UiManager.Instance.Unregister(OnClickClose);
        tributeInventory.Reset();
        tributeSlotList.Reset();
        submitPanel.gameObject.SetActive(false);
        altarPanel.SetActive(false);
    }
}
