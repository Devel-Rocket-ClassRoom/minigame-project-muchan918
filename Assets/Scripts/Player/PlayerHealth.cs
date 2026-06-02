using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDefender
{
    public static event Action OnPlayerDied;
    public static PlayerHealth Instance { get; private set; }

    [Header("Stats")]
    [SerializeField]
    private int maxHp = 100;
    private int currentHp;

    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    [Header("UI")]
    [SerializeField]
    private Slider hpSlider;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

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

    public void AddMaxHp(int amount)
    {
        maxHp += amount;
        currentHp += amount;
        // Debug.Log($"{currentHp}/{maxHp}");
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
        OnPlayerDied?.Invoke();
        GetComponent<PlayerMovement>().SetDead(true);
        animator.SetTrigger("Die");
    }

    public void OnDieAnimationEnd()
    {
        PlayerSpawner.Instance.Respawn(clearInventory: true);
    }

    public void ResetAnimator()
    {
        animator.Rebind();
        animator.Update(0f);
    }
}
