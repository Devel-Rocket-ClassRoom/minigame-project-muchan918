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

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private bool isCheatMode = false;

    private float elapsedTime = 0f;
    private bool midnightTriggered = false;
    private TributeEvent tributeEvent;
    private TileMapGenerator tileMapGenerator;

    public int CurrentDay { get; private set; } = 1;
    public float TotalDayDuration => brightDuration + darkenDuration + nightDuration;
    public float DayProgress => Mathf.Clamp01(elapsedTime / TotalDayDuration);

    public bool IsNight => elapsedTime >= brightDuration + darkenDuration;

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
        tileMapGenerator = GetComponent<TileMapGenerator>();
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

            // 24시 도달 시 리스폰 (치팅 모드면 무시)
            if (!isCheatMode && !midnightTriggered && elapsedTime >= TotalDayDuration)
            {
                midnightTriggered = true;
                PlayerSpawner.Instance.Respawn(clearInventory: true);
                SetMorning();
            }
        }
    }

    public void SetMorning()
    {
        elapsedTime = 0f;
        midnightTriggered = false;
        directionalLight.intensity = maxIntensity;

        if (CurrentDay % 7 == 0)
        {
            if (tributeEvent.Evaluate())
            {
                tributeEvent.AssignNewEvent();
                tileMapGenerator.GenerateMap();
            }
            else
            {
                tributeEvent.AssignNewEvent();
                if (!isCheatMode)
                {
                    gameOverUI.SetActive(true);
                    GamePause.Pause();
                    return;
                }
            }
        }

        CurrentDay++;
    }
}
