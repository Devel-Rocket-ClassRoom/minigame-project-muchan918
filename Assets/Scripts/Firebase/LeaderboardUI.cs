using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject leaderboardPanel;

    [Header("Tab Buttons")]
    [SerializeField] private Button survivedDaysButton;
    [SerializeField] private Button killedAnimalsButton;

    [Header("Scroll View")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject entryItemPrefab;

    [Header("Buttons")]
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button closeButton;

    private LeaderboardType currentType = LeaderboardType.SurvivedDays;

    private void Start()
    {
        survivedDaysButton.onClick.AddListener(() => OnTabClicked(LeaderboardType.SurvivedDays));
        killedAnimalsButton.onClick.AddListener(() => OnTabClicked(LeaderboardType.KilledAnimals));
        refreshButton.onClick.AddListener(() => RefreshLeaderboard().Forget());
        closeButton.onClick.AddListener(ClosePanel);

        LeaderboardManager.Instance.OnLeaderboardUpdated += OnLeaderboardUpdated;
    }

    public void OpenPanel()
    {
        leaderboardPanel.SetActive(true);
        LeaderboardManager.Instance.StartRealtimeListener(currentType);
        RefreshLeaderboard().Forget();
    }

    private void ClosePanel()
    {
        LeaderboardManager.Instance.StopRealtimeListener();
        leaderboardPanel.SetActive(false);
    }

    private void OnTabClicked(LeaderboardType type)
    {
        if (currentType == type)
            return;

        currentType = type;
        LeaderboardManager.Instance.StartRealtimeListener(currentType);
    }

    private async UniTaskVoid RefreshLeaderboard()
    {
        refreshButton.interactable = false;
        var list = await LeaderboardManager.Instance.LoadLeaderboardAsync(currentType);
        RenderEntries(list);
        refreshButton.interactable = true;
    }

    private void OnLeaderboardUpdated(List<LeaderboardEntry> list)
    {
        RenderEntries(list);
    }

    private void RenderEntries(List<LeaderboardEntry> list)
    {
        // 기존 아이템 전부 삭제
        foreach (Transform child in content)
            Destroy(child.gameObject);

        // 새 아이템 생성
        for (int i = 0; i < list.Count; i++)
        {
            LeaderboardEntry entry = list[i];
            GameObject item = Instantiate(entryItemPrefab, content);
            LeaderboardItem leaderboardItem = item.GetComponent<LeaderboardItem>();
            leaderboardItem.SetData(i + 1, entry, currentType);
        }
    }

    private void OnDestroy()
    {
        if (LeaderboardManager.Instance != null)
            LeaderboardManager.Instance.OnLeaderboardUpdated -= OnLeaderboardUpdated;
    }
}