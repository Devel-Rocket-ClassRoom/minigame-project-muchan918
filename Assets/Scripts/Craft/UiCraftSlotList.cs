using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCraftSlotList : MonoBehaviour
{
    public UiCraftSlot prefab;
    public ScrollRect scrollRect;

    private List<UiCraftSlot> slotList = new List<UiCraftSlot>();
    private List<RecipeAsset> recipes = new List<RecipeAsset>();

    public void Setup(List<RecipeAsset> recipeList)
    {
        foreach (var recipe in recipeList)
            AddRecipe(recipe);
    }

    public void AddRecipe(RecipeAsset recipe)
    {
        recipes.Add(recipe);

        var slot = Instantiate(prefab, scrollRect.content);
        slot.slotIndex = slotList.Count;
        slot.SetSlot(recipe);

        slotList.Add(slot);
    }
}
