// CraftInteraction.cs
using System.Collections.Generic;
using UnityEngine;

public class CraftInteraction : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Craft;

    [Header("Data")]
    [SerializeField]
    private List<RecipeAsset> recipes;

    [SerializeField]
    private PlayerInventory playerInventory;

    [Header("UI")]
    [SerializeField]
    private GameObject craftPanel;

    [SerializeField]
    private GameObject ingredientPanel;

    [SerializeField]
    private UiCraftSlotList craftSlotList;

    [SerializeField]
    private UiIngredientSlotList ingredientSlotList;

    private RecipeAsset selectedRecipe;

    private void Awake()
    {
        craftPanel.SetActive(false);
        ingredientPanel.SetActive(false);

        // PlayerInventory 넘겨주기
        craftSlotList.playerInventory = playerInventory;
        ingredientSlotList.playerInventory = playerInventory;

        craftSlotList.Setup(recipes);
    }

    public void Interact(GameObject player)
    {
        craftPanel.SetActive(true);
        craftSlotList.RefreshAvailability();
        GamePause.Pause();
    }

    public void OnClickCraft()
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
        craftSlotList.RefreshAvailability();
        ingredientPanel.SetActive(false);
        selectedRecipe = null;
    }

    public void OnSelectRecipe(RecipeAsset recipe)
    {
        selectedRecipe = recipe;
        ingredientPanel.SetActive(true);
        ingredientSlotList.Setup(recipe);
    }

    public void OnClickClose()
    {
        ingredientPanel.SetActive(false);
        craftPanel.SetActive(false);
        selectedRecipe = null;
        GamePause.Resume();
    }
}
