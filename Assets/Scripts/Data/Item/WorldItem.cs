using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public ItemAsset Asset;

    [SerializeField]
    private float despawnTime = 20f;

    public InteractionType Type => InteractionType.PickUp;

    public void Init(ItemAsset asset)
    {
        Asset = asset;
        Asset.Data = DataTableManager.Get<ItemTable>("ItemTable").Get(Asset.ItemID);
        Destroy(gameObject, despawnTime);
    }

    public void Interact(GameObject player)
    {
        var inventory = player.GetComponent<PlayerInventory>();
        bool success = inventory.AddItem(Asset);
        if (success)
        {
            Destroy(gameObject);
            Debug.Log($"[WorldItem] {Asset.Data.DisplayName} 아이템 습득!");
        }
    }
}
