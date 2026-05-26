using UnityEngine;

[CreateAssetMenu(
    fileName = "HostileAnimalAsset",
    menuName = "Scriptable Objects/HostileAnimalAsset"
)]
public class HostileAnimalAsset : AnimalAsset
{
    public int Damage;
    public float EnterAttackRange = 2f;
    public float ExitAttackRange = 3f;
    public RuntimeAnimatorController AnimatorController;
}
