using Cysharp.Threading.Tasks;
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
    private GameObject gameOverUI;

    [SerializeField]
    private DayTransitionUI dayTransitionUI;

    [SerializeField]
    private Image timerImage;

    [SerializeField]
    private GameSaveController gameSaveController;

    [SerializeField]
    private bool isCheatMode = false;

    [SerializeField]
    private bool isTutorial = false;
    private bool isTutorialDay = false;

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
    public bool IsNight => elapsedTime >= TotalDayDuration * 2f / 3f;
    public int HungerEmptyCount => hungerEmptyCount;

    public int Level { get; private set; } = 1;

    private void Awake()
    {
        tributeEvent = GetComponent<TributeEvent>();
        tributeEvent.SetRequirementIndex(0);
        tileMapGenerator = GetComponent<TileMapGenerator>();
    }

    private void Update()
    {
        if (IsTransitioning || (isTutorial && !isTutorialDay))
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

            if (
                !isCheatMode
                && !isTutorial
                && !isTutorialDay
                && !midnightTriggered
                && elapsedTime >= TotalDayDuration
            )
            {
                midnightTriggered = true;
                UiManager.Instance.CloseAll();
                PlayerHealth.Instance.Die(); // SerializeField 대신 싱글톤
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

        if (!isTutorial && CurrentDay % 7 == 0)
        {
            if (tributeEvent.Evaluate())
            {
                tributeEvent.AssignNewEvent();
                ResourceChunkManager.Instance.ClearDestroyedPositions();
                AnimalChunkManager.Instance.ClearDeadPositions();
                hungerEmptyCount = 0;

                // 맵 재생성(GenerateMap)보다 먼저 업그레이드를 적용해야
                // 업그레이드된 레벨의 동물/자원이 스폰된다.
                // ShowClear 콜백은 실행 순서가 보장되지 않으므로 호출 전에 동기로 처리한다.
                CurrentDay++;
                UpgradeManager.Instance.CheckAutoUpgrade(CurrentDay);

                LoadingUI.Instance.ShowClear(
                    onLoadingComplete: () =>
                    {
                        PlayerHunger.Instance.ResetHunger(); // 싱글톤
                        AnimalChunkManager.Instance.ResetLivingAnimals();
                        gameSaveController.SaveGame();
                        LeaderboardManager.Instance.SaveToLeaderboardAsync(CurrentDay).Forget();
                        timerImage.fillAmount = 0f;
                        LoadingUI.Instance.Hide();
                        IsTransitioning = false;
                        GamePause.Resume();
                        SoundManager.Instance.PlayMainBgm();
                        if (TutorialSceneManager.Instance != null)
                            TutorialSceneManager.Instance.CompleteStep(
                                TutorialStep.ReturnCabinFinal
                            );
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

        if (PlayerHunger.Instance.CurrentHunger == 0) // 싱글톤
            hungerEmptyCount++;

        if (hungerEmptyCount >= 3)
        {
            ShowGameOver();
            return;
        }

        PlayerHunger.Instance.ResetHunger(); // 싱글톤
        AnimalChunkManager.Instance.ResetLivingAnimals();
        CurrentDay++;
        UpgradeManager.Instance.CheckAutoUpgrade(CurrentDay);
        gameSaveController.SaveGame();
        if (!isTutorial)
            LeaderboardManager.Instance.SaveToLeaderboardAsync(CurrentDay).Forget();
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

    public void StartTutorialDay()
    {
        isTutorialDay = true;
        elapsedTime = brightDuration + darkenDuration / 2f;
        timerImage.fillAmount = DayProgress;
    }

    public void SetMorningTutorial()
    {
        elapsedTime = 0f;
        midnightTriggered = false;
        directionalLight.intensity = maxIntensity;
        RenderSettings.ambientLight = dayAmbient;
        timerImage.fillAmount = 0f;
        CurrentDay++;

        if (TutorialSceneManager.Instance != null)
            TutorialSceneManager.Instance.CompleteStep(TutorialStep.StartDay);
    }

    public void SetIsTutorialFalse()
    {
        isTutorial = false;
        isTutorialDay = false;
    }
}
