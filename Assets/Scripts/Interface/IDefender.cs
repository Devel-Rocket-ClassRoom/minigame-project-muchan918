using UnityEngine;

public interface IDefender
{
    int MaxHp { get; }
    int CurrentHp { get; }
    void TakeDamage(int damage, Vector3 hitNormal);
    void Die();
}
