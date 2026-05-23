using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCraftSlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image iconImage;
    public Button button;
    public TextMeshProUGUI nameText;

    private RecipeAsset recipe;
    public RecipeAsset Recipe => recipe;

    public void SetSlot(RecipeAsset recipeAsset)
    {
        recipe = recipeAsset;
        if (recipe.resultItem.Data == null)
            recipe.resultItem.Data = DataTableManager
                .Get<ItemTable>("ItemTable")
                .Get(recipe.resultItem.ItemID);

        iconImage.sprite = recipe.resultItem.Icon;
        nameText.text = recipe.resultItem.Data.DisplayName;
        gameObject.SetActive(true);
    }

    public void SetAvailable(bool available)
    {
        iconImage.color = available ? Color.white : Color.gray;
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }
}
