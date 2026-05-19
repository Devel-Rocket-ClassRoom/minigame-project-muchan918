using Cinemachine;
using UnityEngine;

public class CameraAspectFit : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private float baseDistance = 10f;

    void Start()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_CameraDistance = baseDistance;
    }
}
