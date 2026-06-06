using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScene : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField]
    private AudioClip endingBgm;

    [Header("UI")]
    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private Image madeByImage;

    [SerializeField]
    private Button menuButton;

    private void Start()
    {
        madeByImage.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);

        SoundManager.Instance.StopBgm();
        SoundManager.Instance.bgmSource.clip = endingBgm;
        SoundManager.Instance.bgmSource.loop = false;
        SoundManager.Instance.bgmSource.Play();

        DOTween.Kill(fadeImage); // 추가
        fadeImage
            .DOFade(0f, 3f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(
                    3f,
                    () =>
                    {
                        madeByImage.gameObject.SetActive(true);
                        madeByImage.color = new Color(1f, 1f, 1f, 0f);
                        madeByImage.DOFade(1f, 1f).SetUpdate(true);

                        menuButton.gameObject.SetActive(true);
                        var cg = menuButton.GetComponent<CanvasGroup>();
                        if (cg == null)
                            cg = menuButton.gameObject.AddComponent<CanvasGroup>();
                        cg.alpha = 0f;
                        cg.DOFade(1f, 1f).SetUpdate(true);
                    },
                    true
                );
            });
    }

    public void OnClickMenu()
    {
        SoundManager.Instance.StopBgm();
        SceneManager.LoadScene("TitleScene");
    }
}
