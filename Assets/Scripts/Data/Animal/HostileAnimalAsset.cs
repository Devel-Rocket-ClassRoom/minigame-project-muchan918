using UnityEngine;

[CreateAssetMenu(
    fileName = "HostileAnimalAsset",
    menuName = "Scriptable Objects/HostileAnimalAsset"
)]
public class HostileAnimalAsset : AnimalAsset
{
    public int Damage;
    public float AttackCooldown;
    public float AttackRange = 2f;
    public float RealAttackRange = 3f;
}
