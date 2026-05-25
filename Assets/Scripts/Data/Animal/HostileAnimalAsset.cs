using UnityEngine;

[CreateAssetMenu(
    fileName = "HostileAnimalAsset",
    menuName = "Scriptable Objects/HostileAnimalAsset"
)]
public class HostileAnimalAsset : AnimalAsset
{
    public int Damage;
    public float AttackCooldown;
}
