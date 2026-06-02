using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private List<TutorialData> pages;

    [Header("UI")]
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private Image pageImage;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private TextMeshProUGUI pageText;

    [SerializeField]
    private Button prevButton;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button skipButton;

    [SerializeField]
    private Button completeButton;

    private int _currentPage = 0;

    private void Awake()
    {
        panel.SetActive(false);
        prevButton.onClick.AddListener(OnClickPrev);
        nextButton.onClick.AddListener(OnClickNext);
        skipButton.onClick.AddListener(Hide);
        completeButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        _currentPage = 0;
        panel.SetActive(true);
        GamePause.Pause();
        UpdatePage();
    }

    public void Hide()
    {
        panel.SetActive(false);
        GamePause.Resume();
    }

    private void UpdatePage()
    {
        var data = pages[_currentPage];
        titleText.text = data.title;
        pageImage.sprite = data.image;
        descriptionText.text = data.description;
        pageText.text = $"{_currentPage + 1} / {pages.Count}";

        bool isLastPage = _currentPage == pages.Count - 1;
        prevButton.gameObject.SetActive(_currentPage > 0);
        nextButton.gameObject.SetActive(!isLastPage);
        completeButton.gameObject.SetActive(isLastPage);
    }

    private void OnClickNext()
    {
        if (_currentPage < pages.Count - 1)
        {
            _currentPage++;
            UpdatePage();
        }
    }

    private void OnClickPrev()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            UpdatePage();
        }
    }
}
