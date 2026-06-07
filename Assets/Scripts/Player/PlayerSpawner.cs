using Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance { get; private set; }

    private DayNightCycle dayNightCycle;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private PlayerHealth playerHealth;

    [SerializeField]
    private PlayerHunger playerHunger;

    [SerializeField]
    private PlayerInventory playerInventory;

    [SerializeField]
    private DayTransitionUI dayTransitionUI;

    [SerializeField]
    private CinemachineBrain cinemachineBrain;

    private PlayerMovement playerMovement;

    public Transform PlayerTransform => playerHealth.transform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        dayNightCycle = GetComponent<DayNightCycle>();
        playerMovement = playerHealth.GetComponent<PlayerMovement>();
        if (cinemachineBrain != null)
            cinemachineBrain.m_IgnoreTimeScale = true;

        PlayerHealth.OnDieAnimationEnded += OnDieAnimationEnded;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnDieAnimationEnded -= OnDieAnimationEnded;
    }

    public void Respawn(bool clearInventory = false, bool fullRecover = false)
    {
        dayNightCycle.IsTransitioning = true;
        playerMovement.SetDead(true);

        dayTransitionUI.PlayTransition(
            onMidpoint: () =>
            {
                ApplyRespawn(clearInventory, fullRecover);
                if (TutorialSceneManager.Instance != null)
                    dayNightCycle.SetMorningTutorial();
                else
                    dayNightCycle.SetMorning();
            },
            onFadeInStart: () =>
            {
                playerMovement.SetDead(false);
                dayNightCycle.IsTransitioning = false;
            },
            onComplete: null
        );
    }

    private void OnDieAnimationEnded()
    {
        Respawn(clearInventory: true);
    }

    private void ApplyRespawn(bool clearInventory, bool fullRecover)
    {
        playerHealth.ResetAnimator();
        playerHealth.transform.position = spawnPoint.position;
        playerHealth.transform.rotation = spawnPoint.rotation;
        playerMovement.ResetRotation(spawnPoint.rotation);

        int penalty = Mathf.RoundToInt(
            (1f - (float)playerHunger.CurrentHunger / playerHunger.MaxHunger) * 30f
        );

        if (fullRecover)
            playerHealth.SetHealth(playerHealth.MaxHp - penalty);
        else
            playerHealth.SetHealth(playerHealth.MaxHp / 2 - penalty);

        if (clearInventory)
            playerInventory.SlotList.Clear();
    }
}
