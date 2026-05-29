using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/FoodAsset")]
public class FoodAsset : ScriptableObject
{
    public string FoodID;

    [HideInInspector]
    public FoodData Data;
}
