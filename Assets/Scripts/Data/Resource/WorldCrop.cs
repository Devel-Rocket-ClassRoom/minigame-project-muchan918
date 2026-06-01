using UnityEngine;

public class WorldCrop : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.PickUp;

    [SerializeField]
    private ItemAsset itemAsset;

    private void Start()
    {
        itemAsset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(itemAsset.ItemID);
    }

    public void Interact(GameObject player)
    {
        var inventory = player.GetComponent<PlayerInventory>();
        bool success = inventory.AddItem(itemAsset);
        if (success)
        {
            ResourceChunkManager.Instance.UnregisterResource(gameObject);
            Destroy(gameObject);
        }
    }
}
