using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentAsset", menuName = "Scriptable Objects/EquipmentAsset")]
public class EquipmentAsset : ScriptableObject
{
    public string EquipmentID;

    [HideInInspector]
    public EquipmentData Data;
}
