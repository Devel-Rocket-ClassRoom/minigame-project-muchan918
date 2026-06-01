using UnityEngine;

[CreateAssetMenu(
    fileName = "HostileAnimalAsset",
    menuName = "Scriptable Objects/HostileAnimalAsset"
)]
public class HostileAnimalAsset : AnimalAsset
{
    public int Damage;
    public float EnterAttackRange = 1f;
    public float ExitAttackRange = 1.6f;
    public RuntimeAnimatorController AnimatorController;
}
