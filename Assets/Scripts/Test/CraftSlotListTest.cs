using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftSlotListTest : MonoBehaviour
{
    public UiCraftSlotList craftSlotList;
    public List<RecipeAsset> testRecipes;

    private int currentIndex = 0;

    private void Start()
    {
        craftSlotList.Setup(new List<RecipeAsset>());
    }

    private void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            if (currentIndex < testRecipes.Count)
            {
                craftSlotList.AddRecipe(testRecipes[currentIndex]);
                currentIndex++;
            }
            else
            {
                Debug.Log("더 추가할 레시피가 없어요!");
            }
        }
    }
}
