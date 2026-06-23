using System;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    private static ProfileManager instance;
    public static ProfileManager Instance => instance;

    private DatabaseReference usersRef;

    private string cachedNickname;
    public string CachedNickname => cachedNickname;

    private bool isInitialized = false;
    public bool IsInitialized => isInitialized;

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
            Debug.LogError("[Profile] Firebase 초기화 실패");
            return;
        }

        await UniTask.WaitUntil(() => AuthManager.Instance.IsInitialized);

        usersRef = FirebaseInitializer.Instance.Database.RootReference.Child("users");

        // 이미 로그인 상태면 바로 프로필 로드
        if (AuthManager.Instance.IsLoggedIn)
            await LoadProfileAsync();

        // 이후 로그인 상태 변경 구독
        AuthManager.Instance.LoginStateChanged += OnLoginStateChanged;

        isInitialized = true;
        Debug.Log("[Profile] 초기화 완료");
    }

    private void OnLoginStateChanged(bool signedIn)
    {
        if (signedIn)
            LoadProfileAsync().Forget();
        else
            cachedNickname = null;
    }

    public async UniTask<(bool success, string error)> SaveProfileAsync(string nickname)
    {
        if (!AuthManager.Instance.IsLoggedIn)
            return (false, "로그인이 필요합니다.");

        string userId = AuthManager.Instance.UserId;
        string email = AuthManager.Instance.CurrentUser.Email ?? "";

        try
        {
            Debug.Log("[Profile] 프로필 저장 시도");

            var data = new System.Collections.Generic.Dictionary<string, object>
            {
                { "nickname", nickname },
                { "email", email },
            };

            await usersRef.Child(userId).SetValueAsync(data);
            cachedNickname = nickname;

            Debug.Log($"[Profile] 프로필 저장 완료: {nickname}");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Profile] 프로필 저장 실패: {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async UniTask<(bool success, string error)> LoadProfileAsync()
    {
        if (!AuthManager.Instance.IsLoggedIn)
            return (false, "로그인이 필요합니다.");

        string userId = AuthManager.Instance.UserId;

        try
        {
            Debug.Log("[Profile] 프로필 로드 시도");

            DataSnapshot snapshot = await usersRef.Child(userId).GetValueAsync();

            if (!snapshot.Exists)
            {
                Debug.Log("[Profile] 프로필 없음 (신규 유저)");
                return (false, "프로필이 존재하지 않습니다.");
            }

            cachedNickname = snapshot.Child("nickname").Value?.ToString() ?? "";
            Debug.Log($"[Profile] 프로필 로드 완료: {cachedNickname}");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Profile] 프로필 로드 실패: {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async UniTask<(bool success, string error)> UpdateNicknameAsync(string nickname)
    {
        if (!AuthManager.Instance.IsLoggedIn)
            return (false, "로그인이 필요합니다.");

        string userId = AuthManager.Instance.UserId;

        try
        {
            Debug.Log("[Profile] 닉네임 수정 시도");
            await usersRef.Child(userId).Child("nickname").SetValueAsync(nickname);
            cachedNickname = nickname;
            Debug.Log($"[Profile] 닉네임 수정 완료: {nickname}");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Profile] 닉네임 수정 실패: {ex.Message}");
            return (false, ex.Message);
        }
    }
    
    private void OnDestroy()
    {
        if (AuthManager.Instance != null)
            AuthManager.Instance.LoginStateChanged -= OnLoginStateChanged;

        if (instance == this)
            instance = null;
    }
}
