using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public enum LeaderboardType
{
    SurvivedDays,
    KilledAnimals,
}

public static class LeaderboardTypeExtensions
{
    public static string ToFieldName(this LeaderboardType type)
    {
        return type switch
        {
            LeaderboardType.SurvivedDays  => "survivedDays",
            LeaderboardType.KilledAnimals => "killedAnimals",
            _ => "survivedDays"
        };
    }
}

public class LeaderboardManager : MonoBehaviour
{
    private static LeaderboardManager instance;
    public static LeaderboardManager Instance => instance;

    private DatabaseReference leaderboardRef;
    private Query listenerQuery;
    private EventHandler<ValueChangedEventArgs> currentListener;
    private bool isListenerActive = false;

    private int killedAnimals = 0;
    public int KilledAnimals => killedAnimals;

    public event Action<List<LeaderboardEntry>> OnLeaderboardUpdated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private async UniTaskVoid Start()
    {
        if (!await FirebaseInitializer.Instance.WaitForInitializationAsync())
        {
            Debug.LogError("[Leaderboard] Firebase 초기화 실패");
            return;
        }

        leaderboardRef = FirebaseInitializer.Instance.Database.RootReference.Child("leaderboard");
        GameEvents.AnimalKilled += OnAnimalKilled;

        Debug.Log("[Leaderboard] 초기화 완료");
    }

    private void OnAnimalKilled()
    {
        killedAnimals++;
        Debug.Log($"[Leaderboard] 처치 수: {killedAnimals}");
    }

    // DayNightCycle.SetMorning()에서 호출
    public async UniTask SaveToLeaderboardAsync(int survivedDays)
    {
        if (!AuthManager.Instance.IsLoggedIn)
            return;

        if (leaderboardRef == null)
            return;

        string userId = AuthManager.Instance.UserId;
        string nickname = ProfileManager.Instance.CachedNickname ?? "익명";

        try
        {
            // 기존 데이터 먼저 읽기
            DataSnapshot snapshot = await leaderboardRef.Child(userId).GetValueAsync();

            int bestSurvivedDays = survivedDays;
            int bestKilledAnimals = killedAnimals;

            if (snapshot.Exists)
            {
                int prevSurvivedDays = FirebaseValue.ToInt(snapshot.Child("survivedDays").Value);
                int prevKilledAnimals = FirebaseValue.ToInt(snapshot.Child("killedAnimals").Value);

                // 각각 최고값 유지
                bestSurvivedDays = Mathf.Max(survivedDays, prevSurvivedDays);
                bestKilledAnimals = Mathf.Max(killedAnimals, prevKilledAnimals);
            }

            var data = new Dictionary<string, object>
        {
            { "userId",        userId },
            { "nickname",      nickname },
            { "survivedDays",  bestSurvivedDays },
            { "killedAnimals", bestKilledAnimals },
            { "timestamp",     ServerValue.Timestamp },
        };

            await leaderboardRef.Child(userId).UpdateChildrenAsync(data);
            Debug.Log($"[Leaderboard] 저장 완료 - {bestSurvivedDays}일, {bestKilledAnimals}마리");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Leaderboard] 저장 실패: {ex.Message}");
        }
    }

    public async UniTask<List<LeaderboardEntry>> LoadLeaderboardAsync(LeaderboardType type, int limit = 10)
    {
        if (leaderboardRef == null)
            return new List<LeaderboardEntry>();

        try
        {
            Debug.Log($"[Leaderboard] 로드 시도 - 기준: {type.ToFieldName()}");
            Query query = leaderboardRef.OrderByChild(type.ToFieldName()).LimitToLast(limit);
            DataSnapshot snapshot = await query.GetValueAsync();
            List<LeaderboardEntry> list = ParseEntries(snapshot, type);
            Debug.Log($"[Leaderboard] 로드 완료: {list.Count}개");
            return list;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Leaderboard] 로드 실패: {ex.Message}");
            return new List<LeaderboardEntry>();
        }
    }

    public void StartRealtimeListener(LeaderboardType type, int limit = 10)
    {
        if (leaderboardRef == null)
            return;

        StopRealtimeListener();

        Debug.Log($"[Leaderboard] 실시간 리스너 시작 - 기준: {type.ToFieldName()}");
        listenerQuery = leaderboardRef.OrderByChild(type.ToFieldName()).LimitToLast(limit);
        currentListener = (sender, args) => OnValueChanged(sender, args, type);
        listenerQuery.ValueChanged += currentListener;
        isListenerActive = true;
    }

    public void StopRealtimeListener()
    {
        if (!isListenerActive || listenerQuery == null)
            return;

        Debug.Log("[Leaderboard] 실시간 리스너 중지");
        listenerQuery.ValueChanged -= currentListener;
        currentListener = null;
        listenerQuery = null;
        isListenerActive = false;
    }

    private void OnValueChanged(object sender, ValueChangedEventArgs args, LeaderboardType type)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError($"[Leaderboard] 리스너 오류: {args.DatabaseError}");
            return;
        }

        List<LeaderboardEntry> list = ParseEntries(args.Snapshot, type);
        DispatchUpdateAsync(list).Forget();
    }

    private async UniTaskVoid DispatchUpdateAsync(List<LeaderboardEntry> list)
    {
        await UniTask.SwitchToMainThread();
        OnLeaderboardUpdated?.Invoke(list);
    }

    private List<LeaderboardEntry> ParseEntries(DataSnapshot snapshot, LeaderboardType type)
    {
        List<LeaderboardEntry> list = new List<LeaderboardEntry>();

        if (!snapshot.Exists)
            return list;

        foreach (DataSnapshot child in snapshot.Children)
        {
            var entry = new LeaderboardEntry
            {
                userId        = child.Child("userId").Value?.ToString() ?? "",
                nickname      = child.Child("nickname").Value?.ToString() ?? "",
                survivedDays  = FirebaseValue.ToInt(child.Child("survivedDays").Value),
                killedAnimals = FirebaseValue.ToInt(child.Child("killedAnimals").Value),
            };
            list.Add(entry);
        }

        if (type == LeaderboardType.SurvivedDays)
            list.Sort((a, b) => b.survivedDays.CompareTo(a.survivedDays));
        else if (type == LeaderboardType.KilledAnimals)
            list.Sort((a, b) => b.killedAnimals.CompareTo(a.killedAnimals));

        return list;
    }

    public void ResetKilledAnimals()
    {
        killedAnimals = 0;
        Debug.Log("[Leaderboard] 처치 수 초기화");
    }

    private void OnDestroy()
    {
        StopRealtimeListener();
        GameEvents.AnimalKilled -= OnAnimalKilled;

        if (instance == this)
            instance = null;
    }
}