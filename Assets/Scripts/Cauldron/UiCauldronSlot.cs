using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCauldronSlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image iconImage;
    public Image backgroundImage;
    public Image outlineImage;
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
        Color color = available ? Color.white : Color.gray;
        iconImage.color = color;
        backgroundImage.color = color;
    }

    public void SetSelected(bool selected)
    {
        outlineImage.gameObject.SetActive(selected);
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }
}
