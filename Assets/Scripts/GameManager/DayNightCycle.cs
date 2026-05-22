using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private Light directionalLight;

    [SerializeField]
    private float brightDuration = 110f; // 06:00 ~ 17:00

    [SerializeField]
    private float darkenDuration = 30f; // 17:00 ~ 20:00

    [SerializeField]
    private float nightDuration = 40f; // 20:00 ~ 24:00

    [SerializeField]
    private float maxIntensity = 1f;

    [SerializeField]
    private float minIntensity = 0f;

    [SerializeField]
    private PlayerHealth playerHealth;

    private float elapsedTime = 0f;
    private TributeEvent tributeEvent;

    public int CurrentDay { get; private set; } = 1;
    public float TotalDayDuration => brightDuration + darkenDuration + nightDuration;
    public float DayProgress => Mathf.Clamp01(elapsedTime / TotalDayDuration);

    public string CurrentTimeString
    {
        get
        {
            float totalHours = DayProgress * 18f;
            int hour = 6 + Mathf.FloorToInt(totalHours);
            return $"{hour}:00";
        }
    }

    private void Awake()
    {
        tributeEvent = GetComponent<TributeEvent>();
        tributeEvent.AssignNewEvent();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime <= brightDuration)
        {
            // 낮 - 최대 밝기 유지
            directionalLight.intensity = maxIntensity;
        }
        else if (elapsedTime <= brightDuration + darkenDuration)
        {
            // 노을 - 점점 어두워짐
            float t = (elapsedTime - brightDuration) / darkenDuration;
            directionalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, Mathf.Clamp01(t));
        }
        else
        {
            // 밤 - 최소 밝기 유지
            directionalLight.intensity = minIntensity;
        }
    }

    public void SetMorning()
    {
        elapsedTime = 0f;
        directionalLight.intensity = maxIntensity;

        if (CurrentDay % 7 == 0)
        {
            if (tributeEvent.Evaluate())
                tributeEvent.AssignNewEvent();
            else
            {
                playerHealth.Die();
                return;
            }
        }

        CurrentDay++;
    }
}
