using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSubmitPanel : MonoBehaviour
{
    [Header("Item")]
    public Image icon;
    public TextMeshProUGUI itemName;

    [Header("Count")]
    public Button minusButton;
    public TextMeshProUGUI amountText;
    public Button plusButton;

    public Button submitButton;
    public TributeInventory tributeInventory;

    private int currentAmount;
    private int maxAmount;
    private UiTributeSlot selectedTributeSlot;

    private Dictionary<int, int> submitDict = new Dictionary<int, int>();

    private void Awake()
    {
        gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
    }

    public void Setup(UiTributeSlot tributeSlot, int max)
    {
        selectedTributeSlot = tributeSlot;
        maxAmount = max;
        currentAmount = 0;
        submitDict.Clear();

        icon.sprite = tributeSlot.ItemAsset.Icon;
        itemName.text = tributeSlot.ItemAsset.Data.DisplayName;

        gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        UpdateDisplay();
    }

    public void OnClickPlus()
    {
        if (currentAmount >= maxAmount)
            return;

        int selectedIndex = tributeInventory.GetSelectedSlotIndex();
        if (selectedIndex == -1)
            return;

        if (submitDict.ContainsKey(selectedIndex))
            submitDict[selectedIndex]++;
        else
            submitDict[selectedIndex] = 1;

        currentAmount++;
        UpdateDisplay();
    }

    public void OnClickMinus()
    {
        if (currentAmount <= 0)
            return;

        int selectedIndex = tributeInventory.GetSelectedSlotIndex();
        if (selectedIndex == -1)
            return;

        submitDict[selectedIndex]--;
        if (submitDict[selectedIndex] <= 0)
            submitDict.Remove(selectedIndex);

        currentAmount--;
        UpdateDisplay();
    }

    public void OnClickSubmit()
    {
        tributeInventory.RemoveItems(submitDict);
        selectedTributeSlot.AddSubmit(currentAmount);
        tributeInventory.UpdateSlots();
        gameObject.SetActive(false);
    }

    private void UpdateDisplay()
    {
        amountText.text = currentAmount.ToString();
        submitButton.gameObject.SetActive(currentAmount > 0);
        minusButton.interactable = currentAmount > 0;
        plusButton.interactable = currentAmount < maxAmount;
    }
}
