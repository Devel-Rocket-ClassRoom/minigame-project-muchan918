using UnityEngine;

[CreateAssetMenu(fileName = "DropItemAsset", menuName = "Scriptable Objects/DropItemAsset")]
public class DropItemAsset : ScriptableObject
{
    public string DropItemID;
    public Sprite Icon;

    [HideInInspector]
    public DropItemData Data;
}
