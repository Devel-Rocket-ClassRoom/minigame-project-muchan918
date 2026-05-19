using UnityEngine;

[CreateAssetMenu(fileName = "ResourceAsset", menuName = "Scriptable Objects/ResourceAsset")]
public class ResourceAsset : ScriptableObject
{
    public string ResourceID;
    public GameObject DropPrefab;
    public Sprite Icon;

    [HideInInspector]
    public ResourceData Data;
}
