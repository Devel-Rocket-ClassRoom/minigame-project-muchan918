using DG.Tweening;
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

    [SerializeField]
    private GameObject hungerWarningText; // "음식을 먹어야합니다!" 텍스트 오브젝트

    private Tween blinkTween;

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
        if (hungerSlider != null)
            hungerSlider.value = (float)currentHunger / maxHunger;

        if (hungerWarningText == null)
            return;

        if (currentHunger == 0)
        {
            hungerWarningText.SetActive(true);
            if (blinkTween == null || !blinkTween.IsActive())
            {
                var graphic = hungerWarningText.GetComponent<Graphic>();
                blinkTween = graphic.DOFade(0f, 0.6f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
            }
        }
        else
        {
            blinkTween?.Kill();
            blinkTween = null;
            hungerWarningText.SetActive(false);
        }
    }

    public void SetHunger(int amount)
    {
        currentHunger = Mathf.Clamp(amount, 0, maxHunger);
        UpdateUI();
    }

    public void AddFullHunger()
    {
        currentHunger = maxHunger;
        UpdateUI();
    }
}
