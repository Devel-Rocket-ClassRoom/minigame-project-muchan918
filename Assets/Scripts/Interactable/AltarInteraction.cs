using UnityEngine;
using UnityEngine.UI;

public class AltarInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Altar;

    [SerializeField]
    private TributeEvent tributeEvent;

    [SerializeField]
    private TributeInventory tributeInventory;

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
        GamePause.Pause();
    }

    public void OnClickClose()
    {
        altarPanel.SetActive(false);
        GamePause.Resume();
    }
}
