using UnityEngine;

public class CabinInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Sleep;

    [SerializeField]
    private DayNightCycle dayNightCycle;

    public void Interact(GameObject player)
    {
        if (!dayNightCycle.IsNight)
        {
            Debug.Log("아직 너무 이릅니다!");
            return;
        }
        dayNightCycle.IsTransitioning = true;
        PlayerSpawner.Instance.Respawn(fullRecover: true);
    }
}
