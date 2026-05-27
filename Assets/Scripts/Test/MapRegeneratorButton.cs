using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapRegenerateButton : MonoBehaviour
{
    [SerializeField]
    private TileMapGenerator tileMapGenerator;

    [SerializeField]
    private Button regenerateButton;

    [SerializeField]
    private TextMeshProUGUI statusText;

    private void Start()
    {
        regenerateButton.onClick.AddListener(OnRegenerateClicked);
        SetStatus("대기 중");
    }

    private void OnDestroy()
    {
        regenerateButton.onClick.RemoveListener(OnRegenerateClicked);
    }

    private void OnRegenerateClicked()
    {
        if (GamePause.IsPaused)
            return;

        regenerateButton.interactable = false;
        SetStatus("맵 생성 중...");
        tileMapGenerator.GenerateMap();
        StartCoroutine(WaitForComplete());
    }

    private System.Collections.IEnumerator WaitForComplete()
    {
        yield return null; // GenerateMap이 즉시 Pause를 걸므로 한 프레임 대기
        SetStatus("자원 배치 중...");

        while (GamePause.IsPaused)
            yield return null;

        regenerateButton.interactable = true;
        SetStatus("완료!");
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }
}
