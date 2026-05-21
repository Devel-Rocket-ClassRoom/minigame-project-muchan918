using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject interactionButton;

    [SerializeField]
    private SphereCollider interactionRange;

    [SerializeField]
    private LayerMask interactableLayer;

    private IInteractable currentInteractable;

    private void Awake()
    {
        HideButton();
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(
            interactionRange.bounds.center,
            interactionRange.radius,
            interactableLayer
        );

        if (colliders.Length > 0)
        {
            currentInteractable = colliders[0].GetComponent<IInteractable>();
            if (currentInteractable == null)
            {
                HideButton();
                return;
            }
            ShowButton();
        }
        else
        {
            currentInteractable = null;
            HideButton();
        }
    }

    private void ShowButton()
    {
        interactionButton.SetActive(true);
    }

    private void HideButton()
    {
        interactionButton.SetActive(false);
        currentInteractable = null;
    }

    public void OnClickInteractButton()
    {
        if (currentInteractable != null)
            currentInteractable.Interact(gameObject);
    }
}
