using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float Health { get; private set; }
    public bool IsDead { get; private set; }

    public UnityEvent OnDead;

    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = MaxHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        if (IsDead)
            return;
        Health = Mathf.Clamp(Health + amount, 0f, MaxHealth);
    }

    public virtual void Die()
    {
        IsDead = true;
        OnDead?.Invoke();
    }
}
