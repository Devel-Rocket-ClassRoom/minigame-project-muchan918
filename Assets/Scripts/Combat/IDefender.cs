using UnityEngine;

public interface IDefender
{
    void TakeDamage(int damage, Vector3 hitNormal);
}
