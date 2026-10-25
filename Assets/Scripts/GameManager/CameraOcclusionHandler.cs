using System.Collections.Generic;
using UnityEngine;

public class CameraOcclusionHandler : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private LayerMask canopyLayer;

    [SerializeField]
    private Material originalMaterial;

    [SerializeField]
    private Material transparentMaterial;

    [SerializeField]
    private float sphereRadius = 0.5f;

    private Camera cam;
    private RaycastHit[] hitBuffer = new RaycastHit[32];
    private List<OcclusionTarget> previousTargets = new();
    private List<OcclusionTarget> currentTargets = new();

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 direction = cam.transform.position - player.position;
        float distance = direction.magnitude;

        currentTargets.Clear();

        int hitCount = Physics.SphereCastNonAlloc(
            player.position,
            sphereRadius,
            direction.normalized,
            hitBuffer,
            distance,
            canopyLayer
        );

        for (int i = 0; i < hitCount; i++)
        {
            OcclusionTarget target = hitBuffer[i].collider.GetComponentInParent<OcclusionTarget>();

            if (target != null && !currentTargets.Contains(target))
                currentTargets.Add(target);
        }

        foreach (OcclusionTarget previousTarget in previousTargets)
        {
            if (previousTarget != null && !currentTargets.Contains(previousTarget))
                previousTarget.SetOccluded(false, originalMaterial);
        }

        foreach (OcclusionTarget currentTarget in currentTargets)
        {
            if (currentTarget != null && !previousTargets.Contains(currentTarget))
                currentTarget.SetOccluded(true, transparentMaterial);
        }

        List<OcclusionTarget> temp = previousTargets;
        previousTargets = currentTargets;
        currentTargets = temp;
    }

    private void OnDisable()
    {
        foreach (OcclusionTarget target in previousTargets)
        {
            if (target != null)
                target.SetOccluded(false, originalMaterial);
        }

        previousTargets.Clear();
        currentTargets.Clear();
    }
}
