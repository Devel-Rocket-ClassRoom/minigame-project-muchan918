using TMPro;
using UnityEngine;

public class DateUI : MonoBehaviour
{
    private DayNightCycle dayNightCycle;

    [SerializeField]
    private TMP_Text dateText;

    private void Awake()
    {
        dayNightCycle = GetComponent<DayNightCycle>();
    }

    private void Update()
    {
        dateText.text = $"Day {dayNightCycle.CurrentDay}, {dayNightCycle.CurrentTimeString}";
    }
}
