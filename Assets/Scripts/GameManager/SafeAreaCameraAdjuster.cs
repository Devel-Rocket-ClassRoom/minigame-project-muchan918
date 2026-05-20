using Cinemachine;
using UnityEngine;

[ExecuteAlways]
public class SafeAreaCameraAdjuster : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private float baseDistance = 8f;

    [SerializeField]
    private float distanceAdjustRange = 2f;

    private CinemachineFramingTransposer framingTransposer;
    private Rect lastSafeArea = Rect.zero;
    private Vector2Int lastScreenSize = Vector2Int.zero;

    private void Awake()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void OnEnable()
    {
        Apply();
    }

    private void Update()
    {
        Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);
        if (Screen.safeArea == lastSafeArea && screenSize == lastScreenSize)
            return;

        Apply();
    }

    private void Apply()
    {
        if (framingTransposer == null)
            return;

        Rect safeArea = Screen.safeArea;
        Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

        if (screenSize.x == 0 || screenSize.y == 0)
            return;

        lastSafeArea = safeArea;
        lastScreenSize = screenSize;

        float aspectRatio = (float)screenSize.x / screenSize.y;
        float baseAspect = 16f / 9f;

        // 태블릿(비율 작음) → 양수 → 멀어짐
        // 폰(비율 큼) → 음수 → 가까워짐
        float aspectDiff = baseAspect - aspectRatio;
        float distanceOffset = aspectDiff * distanceAdjustRange;
        distanceOffset = Mathf.Clamp(distanceOffset, -distanceAdjustRange, distanceAdjustRange);

        float finalDistance = baseDistance + distanceOffset;

        Debug.Log(
            $"aspectRatio: {aspectRatio}, distanceOffset: {distanceOffset}, finalDistance: {finalDistance}"
        );

        framingTransposer.m_CameraDistance = finalDistance;
    }
}
