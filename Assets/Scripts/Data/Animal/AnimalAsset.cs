using UnityEngine;

public class AnimalAsset : ScriptableObject
{
    public string AnimalID;
    public GameObject DropPrefab;

    [Header("Idle / Roam")]
    public float IdleDurationMin = 2f;
    public float IdleDurationMax = 4f;
    public float RoamDurationMin = 3f;
    public float RoamDurationMax = 5f;

    [HideInInspector]
    public AnimalData Data;
}
