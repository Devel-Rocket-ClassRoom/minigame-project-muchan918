using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDefender
{
    [Header("Stats")]
    [SerializeField]
    private int maxHp = 100;
    private int currentHp;

    [Header("UI")]
    [SerializeField]
    private Slider hpSlider;

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
    }

    private void UpdateUI()
    {
        if (hpSlider == null)
            return;
        hpSlider.value = (float)currentHp / maxHp;
    }
}
