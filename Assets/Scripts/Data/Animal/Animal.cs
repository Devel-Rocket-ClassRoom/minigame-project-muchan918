using UnityEngine;

public abstract class Animal : MonoBehaviour, IDefender, IDroppable
{
    public AnimalAsset Asset;

    private int currentHp;
    public int MaxHp => Asset.Data.MaxHP;
    public int CurrentHp => currentHp;

    protected virtual void Start()
    {
        Asset.Data = DataTableManager.Get<AnimalTable>("AnimalTable").Get(Asset.AnimalID);
        currentHp = Asset.Data.MaxHP;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Drop()
    {
        if (Asset.DropPrefab != null)
            Instantiate(Asset.DropPrefab, transform.position, Quaternion.identity);
    }

    public void TakeDamage(int damage, Vector3 hitNormal)
    {
        currentHp -= damage;
        OnTakeDamage();

        if (currentHp <= 0)
        {
            Drop();
            Die();
        }
    } // 피격 시 자식 클래스에서 각자 반응 (도망, 공격 등)

    protected abstract void OnTakeDamage();
}
