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

    private void Awake()
    {
        irisMaterial = Instantiate(irisImage.material);
        irisImage.material = irisMaterial;
        irisMaterial.SetFloat("_Radius", 1f);
        irisImage.gameObject.SetActive(false);
    }

    public void PlayTransition(System.Action onMidpoint)
    {
        irisImage.gameObject.SetActive(true);
        playerMovement.SetDead(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(
            DOTween
                .To(
                    () => irisMaterial.GetFloat("_Radius"),
                    v => irisMaterial.SetFloat("_Radius", v),
                    0f,
                    closeDuration
                )
                .SetEase(Ease.InQuart)
        );
        seq.AppendCallback(() => onMidpoint?.Invoke());
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            playerMovement.SetDead(false);
            SoundManager.Instance.PlayTransitionOpen();
        });
        seq.Append(
            DOTween
                .To(
                    () => irisMaterial.GetFloat("_Radius"),
                    v => irisMaterial.SetFloat("_Radius", v),
                    1f,
                    openDuration
                )
                .SetEase(Ease.OutQuart)
        );
        seq.OnComplete(() => irisImage.gameObject.SetActive(false));
    }
}
