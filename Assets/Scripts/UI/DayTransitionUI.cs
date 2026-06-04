using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DayTransitionUI : MonoBehaviour
{
    [SerializeField]
    private Image irisImage;

    [SerializeField]
    private float closeDuration = 0.8f;

    [SerializeField]
    private float openDuration = 0.8f;

    [SerializeField]
    private PlayerMovement playerMovement;

    private Material irisMaterial;
    private Sequence currentSeq;

    private void Awake()
    {
        irisMaterial = Instantiate(irisImage.material);
        irisImage.material = irisMaterial;
        irisMaterial.SetFloat("_Radius", 1f);
        irisImage.gameObject.SetActive(false);
    }

    public void StopTransition()
    {
        if (currentSeq != null)
        {
            currentSeq.Kill();
            currentSeq = null;
        }
    }

    public void PlayTransition(System.Action onMidpoint, System.Action onComplete = null)
    {
        if (irisImage == null)
            return;
        if (currentSeq != null)
            currentSeq.Kill();

        irisImage.gameObject.SetActive(true);
        playerMovement.SetDead(true);
        currentSeq = DOTween.Sequence().SetUpdate(true);
        currentSeq.Append(
            DOTween
                .To(
                    () => irisMaterial.GetFloat("_Radius"),
                    v => irisMaterial.SetFloat("_Radius", v),
                    0f,
                    closeDuration
                )
                .SetEase(Ease.InQuart)
        );
        currentSeq.AppendCallback(() => onMidpoint?.Invoke());
        currentSeq.AppendInterval(1f);
        currentSeq.AppendCallback(() =>
        {
            if (irisImage == null)
                return;
            playerMovement.SetDead(false);
            SoundManager.Instance.PlayTransitionOpen();
        });
        currentSeq.Append(
            DOTween
                .To(
                    () => irisMaterial.GetFloat("_Radius"),
                    v => irisMaterial.SetFloat("_Radius", v),
                    1f,
                    openDuration
                )
                .SetEase(Ease.OutQuart)
        );
        currentSeq.OnComplete(() =>
        {
            if (irisImage == null)
                return;
            irisImage.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void PlayFadeOut(System.Action onComplete)
    {
        if (irisImage == null)
            return;
        if (currentSeq != null)
            currentSeq.Kill();

        irisImage.gameObject.SetActive(true);
        playerMovement.SetDead(true);
        currentSeq = DOTween.Sequence().SetUpdate(true);
        currentSeq.Append(
            DOTween
                .To(
                    () => irisMaterial.GetFloat("_Radius"),
                    v => irisMaterial.SetFloat("_Radius", v),
                    0f,
                    closeDuration
                )
                .SetEase(Ease.InQuart)
        );
        currentSeq.OnComplete(() => onComplete?.Invoke());
    }
}
