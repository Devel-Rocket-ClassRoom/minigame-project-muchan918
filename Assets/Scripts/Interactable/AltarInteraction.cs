using UnityEngine;
using UnityEngine.UI;

public class AltarInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Altar;

    [SerializeField]
    private TributeEvent tributeEvent;

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
    }

    public void OnTributeFulfillButton()
    {
        tributeEvent.IsTributeFulfilled = true;
        Debug.Log("임시: IsTributeFulfilled = true 로 설정됨");
    }

    public void CloseAltarPanel()
    {
        altarPanel.SetActive(false);
    }
}
