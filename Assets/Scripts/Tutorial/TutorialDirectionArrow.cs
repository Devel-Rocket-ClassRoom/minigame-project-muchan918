using UnityEngine;
using UnityEngine.UI;

public class TutorialDirectionArrow : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Image arrowImage;
    public float blinkSpeed = 1.5f;
    public float rotationOffset = 90f;

    private Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - playerMovement.transform.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, -angle + rotationOffset);

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        var color = arrowImage.color;
        color.a = alpha;
        arrowImage.color = color;
    }
}
