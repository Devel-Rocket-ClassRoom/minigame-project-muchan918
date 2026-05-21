using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public ItemAsset Asset;

    public InteractionType Type => InteractionType.PickUp;

    private void Start()
    {
        Asset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(Asset.ItemID);
    }

    public void Interact(GameObject player)
    {
        var inventory = player.GetComponent<PlayerInventory>();
        inventory.AddItem(Asset);
        Destroy(gameObject);
        Debug.Log($"[WorldItem] {Asset.Data.DisplayName} 아이템 습득!");
    }
}
