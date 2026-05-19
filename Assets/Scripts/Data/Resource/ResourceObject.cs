using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public ResourceAsset Asset;

    private int currentHP;

    private void Start()
    {
        Asset.Data = DataTableManager.Get<ResourceTable>("ResourceTable").Get(Asset.ResourceID);
        currentHP = Asset.Data.MaxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Drop();
            Destroy(gameObject);
        }
    }

    private void Drop()
    {
        if (Asset.DropPrefab != null)
            Instantiate(Asset.DropPrefab, transform.position, Quaternion.identity);
    }
}
