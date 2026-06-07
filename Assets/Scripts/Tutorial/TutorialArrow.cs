using UnityEngine;
using UnityEngine.UI;

public class TutorialArrow : MonoBehaviour
{
    public Image arrowImage;
    public float blinkSpeed = 1.5f;

    private void Update()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        var color = arrowImage.color;
        color.a = alpha;
        arrowImage.color = color;
    }
}
