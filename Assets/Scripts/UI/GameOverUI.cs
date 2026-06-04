using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Image bgImage;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private float fadeDuration = 1f;

    private void OnEnable()
    {
        Color c = bgImage.color;
        c.a = 0f;
        bgImage.color = c;
        gameOverPanel.SetActive(false);

        bgImage
            .DOFade(1f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() => StartCoroutine(ShowPanel()));
    }

    private IEnumerator ShowPanel()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameOverPanel.SetActive(true);
    }

    public void OnBackToMenuButton()
    {
        GamePause.Resume();
        SceneManager.LoadScene("TitleScene");
    }
}
