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
        // TODO: 잠자는 연출
        OnWakeUp(player);
    }

    private void OnWakeUp(GameObject player)
    {
        PlayerSpawner.Instance.Respawn(fullRecover: true);
    }
}
