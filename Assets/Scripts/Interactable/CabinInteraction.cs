using UnityEngine;

public class CabinInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Sleep;

    [SerializeField]
    private DayNightCycle dayNightCycle;

    public void Interact(GameObject player)
    {
        // TODO: 잠자는 연출
        OnWakeUp(player);
    }

    private void OnWakeUp(GameObject player)
    {
        player.GetComponent<PlayerHealth>().Recover();
        dayNightCycle.SetMorning();
    }
}
