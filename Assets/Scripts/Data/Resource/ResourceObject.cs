using UnityEngine;

public class ResourceObject : MonoBehaviour, IDefender, IDroppable
{
    public ResourceAsset Asset;

    private int currentHP;

    public int MaxHp => Asset.Data.MaxHP;
    public int CurrentHp => currentHP;

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
            Die();
        }
    }

    public void Drop()
    {
        if (Asset.DropPrefab != null)
            Instantiate(Asset.DropPrefab, transform.position, Quaternion.identity);
    }

    public void Die()
    {
        ResourceChunkManager.Instance.UnregisterResource(gameObject);
        Destroy(gameObject);
    }
}
