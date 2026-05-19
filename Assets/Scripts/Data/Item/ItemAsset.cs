using UnityEngine;

[CreateAssetMenu(fileName = "ItemAsset", menuName = "Scriptable Objects/ItemAsset")]
public class ItemAsset : ScriptableObject
{
    public string ItemID;
    public Sprite Icon;

    [HideInInspector]
    public ItemData Data;
}
