using UnityEngine;

[CreateAssetMenu(fileName = "ResourceAsset", menuName = "Scriptable Objects/ResourceAsset")]
public class ResourceAsset : ScriptableObject
{
    public string ResourceID;
    public ItemAsset DropItem;

    [HideInInspector]
    public ResourceData Data;
}
