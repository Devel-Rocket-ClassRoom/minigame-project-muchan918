// Interaction 모드에 따라 버튼을 다르게 보이게 하기 위함
using UnityEngine;

public enum InteractionType
{
    PickUp,
    Sleep,
    Altar,
    Craft,
    Cook,
    Storage,
}

public interface IInteractable
{
    InteractionType Type { get; }
    void Interact(GameObject player);
}
