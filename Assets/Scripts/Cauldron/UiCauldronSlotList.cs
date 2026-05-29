using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCauldronSlotList : MonoBehaviour
{
    public UiCauldronSlot prefab;
    public ScrollRect scrollRect;
    public PlayerInventory playerInventory;
    public CauldronInteraction cauldronInteraction;

    private List<UiCauldronSlot> slotList = new List<UiCauldronSlot>();
    private List<RecipeAsset> recipes = new List<RecipeAsset>();
    private UiCauldronSlot selectedSlot;

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
            if (selectedSlot != null)
                selectedSlot.SetSelected(false);

            selectedSlot = slotList[capturedIndex];
            selectedSlot.SetSelected(true);

            cauldronInteraction.OnSelectRecipe(recipes[capturedIndex]);
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

    public void ResetSelection()
    {
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);
        selectedSlot = null;
    }
}
