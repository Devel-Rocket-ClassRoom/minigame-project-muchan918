using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public static LoadingUI Instance { get; private set; }

    [Header("Panels")]
    public GameObject initPanel;
    public GameObject clearPanel;

    [Header("Init Panel")]
    public Slider initLoadingBar;
    public TextMeshProUGUI initLoadingPercent;

    [Header("Clear Panel")]
    public Image clearImage;
    public GameObject clearSliderGroup;
    public Slider clearLoadingBar;
    public TextMeshProUGUI clearLoadingText;

    private float currentValue = 0f;
    private string currentLoadingText = "";
    private Coroutine dotAnimCoroutine;
    private System.Action onClearComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (initPanel != null)
            initPanel.SetActive(false);
        if (clearPanel != null)
            clearPanel.SetActive(false);
    }

    public void ShowInit()
    {
        initPanel.SetActive(true);
        if (clearPanel != null)
            clearPanel.SetActive(false);
        currentValue = 0f;
        ApplyProgress(0f);
    }

    public void ShowClear(System.Action onLoadingComplete, System.Action onImageFadeInComplete)
    {
        onClearComplete = onLoadingComplete;

        if (initPanel != null)
            initPanel.SetActive(false);
        clearPanel.SetActive(true);
        clearSliderGroup.SetActive(false);

        Color c = clearImage.color;
        c.a = 0f;
        clearImage.color = c;
        clearImage
            .DOFade(1f, 1f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                clearSliderGroup.SetActive(true);
                currentValue = 0f;
                ApplyProgress(0f);
                onImageFadeInComplete?.Invoke();
            });
    }

    public void Hide()
    {
        if (dotAnimCoroutine != null)
        {
            StopCoroutine(dotAnimCoroutine);
            dotAnimCoroutine = null;
        }
        if (initPanel != null)
            initPanel.SetActive(false);
        if (clearPanel != null)
            clearPanel.SetActive(false);
    }

    public void SetLoadingText(string text)
    {
        currentLoadingText = text;
        if (dotAnimCoroutine != null)
            StopCoroutine(dotAnimCoroutine);
        dotAnimCoroutine = StartCoroutine(DotAnimation());
    }

    private IEnumerator DotAnimation()
    {
        string[] dots = { ".", "..", "..." };
        int index = 0;
        while (true)
        {
            clearLoadingText.text = currentLoadingText + dots[index];
            index = (index + 1) % dots.Length;
            yield return new WaitForSecondsRealtime(0.4f);
        }
    }

    public void SetProgress(float value)
    {
        StartCoroutine(AnimateProgress(value));
    }

    private IEnumerator AnimateProgress(float target)
    {
        while (currentValue < target)
        {
            currentValue = Mathf.MoveTowards(currentValue, target, Time.unscaledDeltaTime * 0.2f);
            ApplyProgress(currentValue);
            yield return null;
        }
        ApplyProgress(target);
        currentValue = target;

        if (target >= 1f)
        {
            yield return new WaitForSecondsRealtime(1f);

            // dot 애니메이션 중지
            if (dotAnimCoroutine != null)
            {
                StopCoroutine(dotAnimCoroutine);
                dotAnimCoroutine = null;
            }

            // ClearPanel 이미지 페이드아웃
            clearImage
                .DOFade(0f, 0.8f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    clearPanel.SetActive(false);
                    onClearComplete?.Invoke(); // DayTransitionUI 페이드인 호출
                });
        }
    }

    private void ApplyProgress(float value)
    {
        if (initPanel.activeSelf)
        {
            initLoadingBar.value = value * 100f;
            initLoadingPercent.text = $"{Mathf.RoundToInt(value * 100)}%";
        }
        else if (clearPanel != null && clearPanel.activeSelf)
        {
            clearLoadingBar.value = value * 100f;
        }
    }
}
