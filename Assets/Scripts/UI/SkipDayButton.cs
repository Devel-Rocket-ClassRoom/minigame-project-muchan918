using System.Collections;
using UnityEngine;

public class SkipDayButton : MonoBehaviour
{
    [SerializeField]
    private DayNightCycle dayNightCycle;

    [SerializeField]
    private TributeEvent tributeEvent;

    [SerializeField]
    private GameObject warningPanel;

    public void OnClickSkip()
    {
        if (!tributeEvent.tributeSlotList.IsAllComplete())
        {
            StopAllCoroutines();
            StartCoroutine(ShowWarning());
            return;
        }

        int currentCycleCap = ((dayNightCycle.CurrentDay - 1) / 7 + 1) * 7;
        if (dayNightCycle.CurrentDay >= currentCycleCap)
            return;

        dayNightCycle.SetDay(dayNightCycle.CurrentDay + 1);
    }

    private IEnumerator ShowWarning()
    {
        warningPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningPanel.SetActive(false);
    }
}
