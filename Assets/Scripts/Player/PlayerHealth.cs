using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDefender
{
    [Header("Stats")]
    [SerializeField]
    private int maxHp = 100;
    private int currentHp;

    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    [Header("UI")]
    [SerializeField]
    private Slider hpSlider;

    // [SerializeField]
    // private GameObject gameOverUI;

    private void Start()
    {
        currentHp = maxHp;
        UpdateUI();
    }

    private void Update()
    {
        // 테스트용
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            TakeDamage(10, Vector3.zero);
        }
    }

    public void TakeDamage(int damage, Vector3 hitNormal)
    {
        currentHp = Mathf.Max(0, currentHp - damage);
        UpdateUI();

        Debug.Log($"HP: {currentHp}/{maxHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Recover()
    {
        currentHp = maxHp;
        UpdateUI();
    }

    public void Recover(int amount)
    {
        currentHp = Mathf.Min(currentHp + amount, maxHp);
        UpdateUI();
    }

    public void SetHealth(int amount)
    {
        currentHp = Mathf.Clamp(amount, 0, maxHp);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (hpSlider == null)
            return;
        hpSlider.value = (float)currentHp / maxHp;
    }

    public void Die()
    {
        Debug.Log("죽음");
        // gameOverUI.SetActive(true);
        // GamePause.Pause();
        PlayerSpawner.Instance.Respawn();
    }
}
