using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour, IUpgradeable
{
    [SerializeField]
    private Light directionalLight;

    [SerializeField]
    private float brightDuration = 110f;

    [SerializeField]
    private float darkenDuration = 30f;

    [SerializeField]
    private float nightDuration = 40f;

    [SerializeField]
    private float maxIntensity = 1f;

    [SerializeField]
    private float minIntensity = 0f;

    [SerializeField]
    private Color dayAmbient = new Color(0.4f, 0.4f, 0.4f);

    [SerializeField]
    private Color nightAmbient = new Color(0.02f, 0.02f, 0.05f);

    [SerializeField]
    private PlayerHealth playerHealth;

    [SerializeField]
    private PlayerHunger playerHunger;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private DayTransitionUI dayTransitionUI;

    [SerializeField]
    private Image timerImage;

    [SerializeField]
    private GameSaveController gameSaveController;

    [SerializeField]
    private bool isCheatMode = false;

    private float elapsedTime = 0f;
    private bool midnightTriggered = false;
    private int hungerEmptyCount = 0;
    private bool pendingDurationUpgrade = false;
    private TributeEvent tributeEvent;
    private TileMapGenerator tileMapGenerator;

    public int CurrentDay { get; private set; } = 1;
    public bool IsTransitioning { get; set; } = false;
    public float TotalDayDuration => brightDuration + darkenDuration + nightDuration;
    public float DayProgress => Mathf.Clamp01(elapsedTime / TotalDayDuration);
    public bool IsNight => elapsedTime >= brightDuration + darkenDuration;
    public int HungerEmptyCount => hungerEmptyCount;

    public string CurrentTimeString
    {
        get
        {
            float totalHours = DayProgress * 18f;
            int hour = 6 + Mathf.FloorToInt(totalHours);
            return $"{hour}:00";
        }
    }

    public int Level { get; private set; } = 1;

    private void Awake()
    {
        tributeEvent = GetComponent<TributeEvent>();
        tributeEvent.SetRequirementIndex(0);
        tileMapGenerator = GetComponent<TileMapGenerator>();
    }

    private void Update()
    {
        if (IsTransitioning)
            return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime <= brightDuration)
        {
            directionalLight.intensity = maxIntensity;
            RenderSettings.ambientLight = dayAmbient;
        }
        else if (elapsedTime <= brightDuration + darkenDuration)
        {
            float t = (elapsedTime - brightDuration) / darkenDuration;
            float clamped = Mathf.Clamp01(t);
            directionalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, clamped);
            RenderSettings.ambientLight = Color.Lerp(dayAmbient, nightAmbient, clamped);
        }
        else
        {
            directionalLight.intensity = minIntensity;
            RenderSettings.ambientLight = nightAmbient;

            if (!isCheatMode && !midnightTriggered && elapsedTime >= TotalDayDuration)
            {
                midnightTriggered = true;
                UiManager.Instance.CloseAll();
                PlayerHealth.Instance.Die();
            }
        }

        timerImage.fillAmount = DayProgress;
    }

    private void ShowGameOver()
    {
        IsTransitioning = true;
        dayTransitionUI.StopTransition();
        dayTransitionUI.PlayFadeOut(() =>
        {
            gameOverUI.SetActive(true);
            GamePause.Pause();
        });
    }

    public void SetMorning()
    {
        UiManager.Instance.CloseAll();
        elapsedTime = 0f;
        midnightTriggered = false;
        directionalLight.intensity = maxIntensity;
        RenderSettings.ambientLight = dayAmbient;

        if (pendingDurationUpgrade)
        {
            brightDuration += 5f;
            darkenDuration += 5f;
            nightDuration += 5f;
            pendingDurationUpgrade = false;
        }

        if (CurrentDay % 7 == 0)
        {
            if (tributeEvent.Evaluate())
            {
                tributeEvent.AssignNewEvent();
                ResourceChunkManager.Instance.ClearDestroyedPositions();
                AnimalChunkManager.Instance.ClearDeadPositions();
                hungerEmptyCount = 0;

                LoadingUI.Instance.ShowClear(
                    onLoadingComplete: () =>
                    {
                        playerHunger.ResetHunger();
                        AnimalChunkManager.Instance.ResetLivingAnimals();
                        CurrentDay++;
                        UpgradeManager.Instance.CheckAutoUpgrade(CurrentDay);
                        gameSaveController.SaveGame();
                        timerImage.fillAmount = 0f;
                        LoadingUI.Instance.Hide();
                        IsTransitioning = false;
                        GamePause.Resume();
                        SoundManager.Instance.PlayMainBgm();
                    },
                    onImageFadeInComplete: () =>
                    {
                        tileMapGenerator.GenerateMap(clear: true);
                    }
                );
                return;
            }
            else
            {
                tributeEvent.AssignNewEvent();
                if (!isCheatMode)
                {
                    ShowGameOver();
                    return;
                }
            }
        }

        if (playerHunger.CurrentHunger == 0)
            hungerEmptyCount++;

        if (hungerEmptyCount >= 3)
        {
            ShowGameOver();
            return;
        }

        playerHunger.ResetHunger();
        AnimalChunkManager.Instance.ResetLivingAnimals();
        CurrentDay++;
        UpgradeManager.Instance.CheckAutoUpgrade(CurrentDay);
        gameSaveController.SaveGame();
        //IsTransitioning = false;
    }

    public void SetDay(int day)
    {
        CurrentDay = day;
    }

    public void SetHungerEmptyCount(int count)
    {
        hungerEmptyCount = count;
    }

    public void Upgrade()
    {
        if (Level >= 2)
            return;
        Level++;
        pendingDurationUpgrade = true;
    }
}
