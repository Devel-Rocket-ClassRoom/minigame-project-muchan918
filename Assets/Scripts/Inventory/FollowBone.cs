using UnityEngine;

public class FollowBone : MonoBehaviour
{
    [SerializeField]
    private Transform bone;

    [SerializeField]
    private Vector3 positionOffset;

    [SerializeField]
    private Vector3 rotationOffset;

    private void LateUpdate()
    {
        if (bone == null)
            return;
        transform.position = bone.position + bone.TransformDirection(positionOffset);
        transform.rotation = bone.rotation * Quaternion.Euler(rotationOffset);
    }
}
