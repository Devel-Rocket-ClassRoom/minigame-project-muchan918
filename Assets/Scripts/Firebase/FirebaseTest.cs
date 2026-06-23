using Cysharp.Threading.Tasks;
using UnityEngine;

public class FirebaseTest : MonoBehaviour
{
    private async UniTaskVoid Start()
    {
        await FirebaseInitializer.Instance.WaitForInitializationAsync();
        Debug.Log("[Test] Firebase 초기화 완료");

        await UniTask.WaitUntil(() => AuthManager.Instance.IsInitialized);
        await UniTask.WaitUntil(() => ProfileManager.Instance.IsInitialized);
        Debug.Log("[Test] Manager 초기화 완료");

        AuthManager.Instance.SignOut();
        Debug.Log("[Test] 로그아웃 완료");
    }
}