// UiProgressPanel.cs
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiProgressPanel : MonoBehaviour
{
    [SerializeField]
    private TributeEvent tributeEvent;

    [SerializeField]
    private DayNightCycle dayNightCycle;

    [SerializeField]
    private UiProgressSlot slotPrefab;

    [SerializeField]
    private Transform slotParent;

    [SerializeField]
    private TextMeshProUGUI hungerText;

    private List<UiProgressSlot> slotList = new List<UiProgressSlot>();

    private bool isOpen;

    private void Awake()
    {
        gameObject.SetActive(false);
        isOpen = false;
    }

    private void Refresh()
    {
        // 슬롯 초기화
        foreach (var slot in slotList)
            Destroy(slot.gameObject);
        slotList.Clear();

        // 제단 슬롯 생성
        var slotInfoList = tributeEvent.tributeSlotList.GetSlotInfoList();
        var requirement = tributeEvent.levelPool[tributeEvent.CurrentEventLevel].requirements[
            tributeEvent.CurrentRequirementIndex
        ];

        for (int i = 0; i < requirement.requiredItems.Count; i++)
        {
            var slot = Instantiate(slotPrefab, slotParent);
            var entry = requirement.requiredItems[i];
            int submitted = i < slotInfoList.Count ? slotInfoList[i].submitted : 0;
            slot.SetSlot(entry.item, submitted, entry.amount, slotInfoList[i].isComplete);
            slotList.Add(slot);
        }

        // 굶주림
        int count = dayNightCycle.HungerEmptyCount;
        hungerText.text = $"굶주림: {count} / 3";
    }

    public void OnClickProgress()
    {
        isOpen = !isOpen;
        gameObject.SetActive(isOpen);

        if (isOpen)
        {
            Refresh();
            UiManager.Instance.Register(Close);
        }
        else
        {
            UiManager.Instance.Unregister(Close);
        }
    }

    private void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
        UiManager.Instance.Unregister(Close);
    }
}
