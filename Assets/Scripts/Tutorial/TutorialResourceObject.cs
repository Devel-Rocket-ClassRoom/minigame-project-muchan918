using UnityEngine;

public class TutorialResourceObject : MonoBehaviour, IDefender, IDroppable
{
    public ResourceAsset asset;
    public TutorialStep completeStep;

    private int currentHP;

    public int MaxHp => asset.Data.MaxHP;
    public int CurrentHp => currentHP;

    private void Start()
    {
        asset.Data = DataTableManager.Get<ResourceTable>("ResourceTable").Get(asset.ResourceID);
        currentHP = asset.Data.MaxHP;
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector3.zero);
    }

    public void TakeDamage(int damage, Vector3 hitNormal)
    {
        currentHP -= damage;
        SoundManager.Instance.PlayResourceHit();

        if (currentHP <= 0)
        {
            Drop();
            Die();
        }
    }

    public void Drop()
    {
        if (asset.DropItem == null)
            return;

        var go = Instantiate(
            WorldItemManager.Instance.WorldItemPrefab,
            transform.position,
            Quaternion.identity
        );
        go.GetComponent<WorldItem>().Init(asset.DropItem);
    }

    public void Die()
    {
        TutorialSceneManager.Instance.CompleteStep(completeStep);
        Destroy(gameObject);
    }
}
