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
    }

    public void Respawn(bool clearInventory = false, bool fullRecover = false)
    {
        playerHealth.transform.position = spawnPoint.position;
        playerHealth.transform.rotation = spawnPoint.rotation;

        int penalty = Mathf.RoundToInt(
            (1f - (float)playerHunger.CurrentHunger / playerHunger.MaxHunger) * 30f
        );

        if (fullRecover)
            playerHealth.SetHealth(playerHealth.MaxHp - penalty);
        else
            playerHealth.SetHealth(playerHealth.MaxHp / 2 - penalty);

        if (clearInventory)
            playerInventory.SlotList.Clear();

        dayNightCycle.SetMorning();
    }
}
