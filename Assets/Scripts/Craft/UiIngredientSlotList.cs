using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiIngredientSlotList : MonoBehaviour
{
    public UiIngredientSlot prefab;
    public ScrollRect scrollRect;
    public PlayerInventory playerInventory;

    private List<UiIngredientSlot> slotList = new List<UiIngredientSlot>();

    public void Setup(RecipeAsset recipe)
    {
        // 기존 슬롯 제거
        foreach (var slot in slotList)
            Destroy(slot.gameObject);
        slotList.Clear();

        // 재료 슬롯 생성
        foreach (var ingredient in recipe.ingredients)
        {
            var slot = Instantiate(prefab, scrollRect.content);
            int owned = playerInventory.SlotList.GetTotalAmount(ingredient.item.ItemID);
            slot.SetSlot(ingredient.item, ingredient.amount, owned);
            slotList.Add(slot);
        }
    }

    public bool IsAllFulfilled()
    {
        foreach (var slot in slotList)
            if (!slot.IsFulfilled)
                return false;
        return true;
    }
}
