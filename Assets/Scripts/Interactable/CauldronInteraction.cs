using System.Collections.Generic;
using UnityEngine;

public class CauldronInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Cook;

    [Header("Data")]
    [SerializeField]
    private List<RecipeAsset> recipes;

    [SerializeField]
    private PlayerInventory playerInventory;

    [Header("UI")]
    [SerializeField]
    private GameObject cauldronPanel;

    [SerializeField]
    private GameObject ingredientPanel;

    [SerializeField]
    private UiCauldronSlotList cauldronSlotList;

    [SerializeField]
    private UiCauldronIngredientSlotList ingredientSlotList;

    private RecipeAsset selectedRecipe;

    private void Awake()
    {
        cauldronPanel.SetActive(false);
        ingredientPanel.SetActive(false);

        cauldronSlotList.playerInventory = playerInventory;
        ingredientSlotList.playerInventory = playerInventory;
        cauldronSlotList.cauldronInteraction = this;

        cauldronSlotList.Setup(recipes);
    }

    public void Interact(GameObject player)
    {
        cauldronPanel.SetActive(true);
        cauldronSlotList.RefreshAvailability();
    }

    public void OnSelectRecipe(RecipeAsset recipe)
    {
        selectedRecipe = recipe;
        ingredientPanel.SetActive(true);
        ingredientSlotList.Setup(recipe);
    }

    public void OnClickCook()
    {
        if (selectedRecipe == null)
            return;
        if (!ingredientSlotList.IsAllFulfilled())
            return;

        // 재료 차감
        foreach (var ingredient in selectedRecipe.ingredients)
            playerInventory.SlotList.RemoveItemByAsset(ingredient.item, ingredient.amount);

        // 결과물 추가
        for (int i = 0; i < selectedRecipe.resultAmount; i++)
            playerInventory.AddItem(selectedRecipe.resultItem);

        // 갱신
        cauldronSlotList.RefreshAvailability();
        ingredientPanel.SetActive(false);
        selectedRecipe = null;
    }

    public void OnClickClose()
    {
        ingredientPanel.SetActive(false);
        cauldronPanel.SetActive(false);
        cauldronSlotList.ResetSelection();
        selectedRecipe = null;
    }
}
