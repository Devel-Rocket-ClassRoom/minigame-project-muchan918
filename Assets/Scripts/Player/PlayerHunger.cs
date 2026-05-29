using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHunger : MonoBehaviour
{
    public static PlayerHunger Instance { get; private set; }

    [Header("Stats")]
    [SerializeField]
    private int maxHunger;
    private int currentHunger;

    public int CurrentHunger => currentHunger;
    public int MaxHunger => maxHunger;

    [Header("UI")]
    [SerializeField]
    private Slider hungerSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        maxHunger = 100;
        currentHunger = 0;
        UpdateUI();
    }

    private void Update()
    {
        // 테스트용: K키를 누르면 배고픔 30 회복
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            AddHunger(30);
        }
    }

    public void AddHunger(int amount)
    {
        currentHunger = Mathf.Min(currentHunger + amount, maxHunger);
        UpdateUI();
        Debug.Log($"배고픔: {currentHunger}/{maxHunger}");
    }

    public void ResetHunger()
    {
        currentHunger = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (hungerSlider == null)
            return;
        hungerSlider.value = (float)currentHunger / maxHunger;
    }
}
