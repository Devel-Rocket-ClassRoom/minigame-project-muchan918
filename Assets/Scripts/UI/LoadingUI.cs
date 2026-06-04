using System.Collections;
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
    public Slider clearLoadingBar;
    public TextMeshProUGUI clearLoadingPercent;

    private float currentValue = 0f;

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

    public void ShowClear()
    {
        if (initPanel != null)
            initPanel.SetActive(false);
        clearPanel.SetActive(true);
        currentValue = 0f;
        ApplyProgress(0f);
    }

    public void Hide()
    {
        if (initPanel != null)
            initPanel.SetActive(false);
        if (clearPanel != null)
            clearPanel.SetActive(false);
    }

    public void SetProgress(float value)
    {
        StartCoroutine(AnimateProgress(value));
    }

    private IEnumerator AnimateProgress(float target)
    {
        while (currentValue < target)
        {
            currentValue = Mathf.MoveTowards(currentValue, target, Time.unscaledDeltaTime * 0.5f);
            ApplyProgress(currentValue);
            yield return null;
        }
        ApplyProgress(target);
        currentValue = target;

        if (target >= 1f)
            yield return new WaitForSecondsRealtime(1.5f);
    }

    private void ApplyProgress(float value)
    {
        string percent = $"{Mathf.RoundToInt(value * 100)}%";

        if (initPanel.activeSelf)
        {
            initLoadingBar.value = value * 100f;
            initLoadingPercent.text = percent;
        }
        else if (clearPanel != null && clearPanel.activeSelf)
        {
            clearLoadingBar.value = value * 100f;
            clearLoadingPercent.text = percent;
        }
    }
}
