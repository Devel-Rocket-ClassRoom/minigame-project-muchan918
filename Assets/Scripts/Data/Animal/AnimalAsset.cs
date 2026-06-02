using UnityEngine;

public class AnimalAsset : ScriptableObject
{
    public string AnimalID;
    public ItemAsset DropItem;

    [Header("Idle / Roam")]
    public float IdleDurationMin = 2f;
    public float IdleDurationMax = 4f;

    [HideInInspector]
    public AnimalData Data;
}
