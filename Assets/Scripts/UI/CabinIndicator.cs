using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CabinIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform cabin;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private RectTransform panelRect;

    [SerializeField]
    private Image indicatorImage;

    [SerializeField]
    private TextMeshProUGUI distanceText;

    [SerializeField]
    private float edgePadding = 20f;

    [SerializeField]
    private float textOffset = 20f;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(cabin.position);

        if (screenPos.z < 0)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
            screenPos.z = 1f;
        }

        Vector3 viewportPos = cam.ScreenToViewportPoint(screenPos);
        float ndcX = viewportPos.x * 2f - 1f;
        float ndcY = viewportPos.y * 2f - 1f;

        bool isOffScreen = Mathf.Abs(ndcX) > 1f || Mathf.Abs(ndcY) > 1f;

        if (isOffScreen)
        {
            Vector3 local = cam.transform.InverseTransformPoint(cabin.position);
            Vector2 dir = new Vector2(local.x, local.y).normalized;

            float halfW = panelRect.rect.width * 0.5f - edgePadding;
            float halfH = panelRect.rect.height * 0.5f - edgePadding;

            float scaleX = Mathf.Abs(dir.x) > 0.001f ? halfW / Mathf.Abs(dir.x) : float.MaxValue;
            float scaleY = Mathf.Abs(dir.y) > 0.001f ? halfH / Mathf.Abs(dir.y) : float.MaxValue;

            Vector2 edgePos = dir * Mathf.Min(scaleX, scaleY);

            indicatorImage.rectTransform.anchoredPosition = edgePos;
            indicatorImage.gameObject.SetActive(true);

            distanceText.rectTransform.anchoredPosition = edgePos + Vector2.down * textOffset;
            int distance = Mathf.RoundToInt(Vector3.Distance(player.position, cabin.position));
            distanceText.text = $"{distance}m";
            distanceText.gameObject.SetActive(true);
        }
        else
        {
            indicatorImage.gameObject.SetActive(false);
            distanceText.gameObject.SetActive(false);
        }
    }
}
