using UnityEngine;

public class AnimalAsset : ScriptableObject
{
    public string AnimalID;
    public GameObject DropPrefab;

    [Header("Idle / Roam")]
    public float IdleDurationMin = 2f;
    public float IdleDurationMax = 4f;

    [HideInInspector]
    public AnimalData Data;
}
