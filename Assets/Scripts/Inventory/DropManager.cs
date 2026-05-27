using TMPro;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [SerializeField]
    private UiInventorySlotList inventorySlotList;

    [SerializeField]
    private GameObject dropPopup;

    [SerializeField]
    private TextMeshProUGUI dropAmountText;

    private int dropAmount;
    private int maxAmount;

    private void Awake()
    {
        dropPopup.SetActive(false);
    }

    public void OpenPopup(int amount)
    {
        maxAmount = amount;
        dropAmount = 1;
        dropAmountText.text = dropAmount.ToString();
        dropPopup.SetActive(true);
    }

    public void OnClickPlus()
    {
        if (dropAmount >= maxAmount)
            return;

        dropAmount++;
        dropAmountText.text = dropAmount.ToString();
    }

    public void OnClickMinus()
    {
        if (dropAmount <= 1)
            return;

        dropAmount--;
        dropAmountText.text = dropAmount.ToString();
    }

    public void OnClickHalf()
    {
        dropAmount = Mathf.Max(1, Mathf.FloorToInt(maxAmount / 2f));
        dropAmountText.text = dropAmount.ToString();
    }

    public void OnClickAll()
    {
        dropAmount = maxAmount;
        dropAmountText.text = dropAmount.ToString();
    }

    public void OnClickConfirm()
    {
        inventorySlotList.RemoveItem(dropAmount);
        dropPopup.SetActive(false);
    }

    public void OnClickCancel()
    {
        dropPopup.SetActive(false);
    }
}
