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
    private PlayerInventory playerInventory;

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

    public void Respawn(bool fullRecover = false)
    {
        playerHealth.transform.position = spawnPoint.position;

        if (fullRecover)
            playerHealth.Recover();
        else
            playerHealth.SetHealth(playerHealth.MaxHp / 2);

        playerInventory.SlotList.Clear();
        dayNightCycle.SetMorning();
    }
}
