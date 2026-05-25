using UnityEngine;

public class AnimalAsset : ScriptableObject
{
    public string AnimalID;
    public GameObject DropPrefab;

    [HideInInspector]
    public AnimalData Data;
}
