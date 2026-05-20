using UnityEngine;

[ExecuteAlways]
// Rect가 있으면 넣어주고 없으면 안넣고 뭐 제약있고 그럼
[RequireComponent(typeof(RectTransform))]
public class SafeAreaPanel : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;

    private Rect lastSafeArea = Rect.zero;
    private Vector2Int lastScreenSize = Vector2Int.zero;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        Apply();
    }

    private void OnRectTransformDimensionsChange()
    {
        // isActiveAndEnabled 이게 true면 화면에 나와있다
        if (!isActiveAndEnabled)
            return;

        Apply();
    }

    private void Apply()
    {
        Rect pixelRect = parentCanvas.pixelRect;
        Rect safeArea = Screen.safeArea;
        Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

        if (safeArea == lastSafeArea && screenSize == lastScreenSize)
            return;

        lastSafeArea = safeArea;
        lastScreenSize = screenSize;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= pixelRect.width;
        anchorMin.y /= pixelRect.height;
        anchorMax.x /= pixelRect.width;
        anchorMax.y /= pixelRect.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // 앵커가 바뀌면 포지션도 상대위치로 바뀌기 때문에 초기화 해줘야함
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
    }
}
