using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class StatusPanelSizer : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float height = rect.rect.height;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height * 3f);
    }
}
