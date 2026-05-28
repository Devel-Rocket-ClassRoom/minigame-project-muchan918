using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiUpgradeIngredientSlotList : MonoBehaviour
{
    public UiIngredientSlot prefab;
    public ScrollRect scrollRect;
    public PlayerInventory playerInventory;

    private List<UiIngredientSlot> slotList = new List<UiIngredientSlot>();

    public void Setup(UpgradeAsset asset, int level)
    {
        foreach (var slot in slotList)
            Destroy(slot.gameObject);
        slotList.Clear();

        if (level >= asset.MaxLevel)
            return;

        foreach (var ingredient in asset.costPerLevel[level].ingredients)
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
