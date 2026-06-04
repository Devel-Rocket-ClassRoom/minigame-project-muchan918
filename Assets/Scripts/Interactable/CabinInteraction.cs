using System.Collections;
using UnityEngine;

public class CabinInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Sleep;

    [SerializeField]
    private DayNightCycle dayNightCycle;

    [SerializeField]
    private GameObject tooEarlyPopup;

    [SerializeField]
    private float popupDuration = 1f;

    private Coroutine popupCoroutine;

    private void Awake()
    {
        tooEarlyPopup.SetActive(false);
    }

    public void Interact(GameObject player)
    {
        if (!dayNightCycle.IsNight)
        {
            ShowPopup();
            return;
        }
        dayNightCycle.IsTransitioning = true;
        PlayerSpawner.Instance.Respawn(fullRecover: true);
    }

    private void ShowPopup()
    {
        if (popupCoroutine != null)
            StopCoroutine(popupCoroutine);
        popupCoroutine = StartCoroutine(PopupRoutine());
    }

    private IEnumerator PopupRoutine()
    {
        tooEarlyPopup.SetActive(true);
        yield return new WaitForSecondsRealtime(popupDuration);
        tooEarlyPopup.SetActive(false);
        popupCoroutine = null;
    }
}
