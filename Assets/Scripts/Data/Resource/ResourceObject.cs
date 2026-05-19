using UnityEngine;

public class ResourceObject : MonoBehaviour, IDefender, IDroppable
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
        TakeDamage(damage, Vector3.zero);
    }

    public void TakeDamage(int damage, Vector3 hitNormal)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Drop();
            Destroy(gameObject);
        }
    }

    public void Drop()
    {
        if (Asset.DropPrefab != null)
            Instantiate(Asset.DropPrefab, transform.position, Quaternion.identity);
    }
}
