using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "7Days/RecipeAsset")]
public class RecipeAsset : ScriptableObject
{
    [System.Serializable]
    public class Ingredient
    {
        public ItemAsset item;
        public int amount;
    }

    public ItemAsset resultItem;
    public int resultAmount = 1;
    public List<Ingredient> ingredients;
}
