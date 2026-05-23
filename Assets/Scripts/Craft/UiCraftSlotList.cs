using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCraftSlotList : MonoBehaviour
{
    public UiCraftSlot prefab;
    public ScrollRect scrollRect;
    public PlayerInventory playerInventory;
    public CraftInteraction craftInteraction;

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
        slot.SetAvailable(IsAvailable(recipe));

        int capturedIndex = slotList.Count;
        slot.button.onClick.AddListener(() =>
        {
            craftInteraction.OnSelectRecipe(recipes[capturedIndex]);
        });

        slotList.Add(slot);
    }

    public void RefreshAvailability()
    {
        for (int i = 0; i < slotList.Count; i++)
            slotList[i].SetAvailable(IsAvailable(recipes[i]));
    }

    private bool IsAvailable(RecipeAsset recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            int owned = playerInventory.SlotList.GetTotalAmount(ingredient.item.ItemID);
            if (owned < ingredient.amount)
                return false;
        }
        return true;
    }
}
