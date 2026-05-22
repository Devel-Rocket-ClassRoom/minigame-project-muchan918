using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "7Days/TributeRequirement")]
public class TributeRequirement : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public ItemAsset item;
        public int amount;
    }

    public List<Entry> requiredItems;
}
