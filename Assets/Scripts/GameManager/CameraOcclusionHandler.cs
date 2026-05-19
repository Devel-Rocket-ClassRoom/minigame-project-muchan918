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
    private HashSet<Renderer> fadedObjects = new();

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HashSet<Renderer> hitRenderers = new();

        Vector3 direction = cam.transform.position - player.position;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.SphereCastAll(
            player.position,
            sphereRadius,
            direction.normalized,
            distance,
            canopyLayer
        );

        foreach (var hit in hits)
        {
            Renderer[] renderers = hit.collider.GetComponentsInChildren<Renderer>();

            foreach (var rend in renderers)
            {
                hitRenderers.Add(rend);

                if (!fadedObjects.Contains(rend))
                {
                    if (rend != null)
                        rend.material = transparentMaterial;
                    fadedObjects.Add(rend);
                }
            }
        }

        List<Renderer> toRestore = new();
        foreach (var rend in fadedObjects)
        {
            if (!hitRenderers.Contains(rend))
            {
                if (rend != null)
                    rend.material = originalMaterial;
                toRestore.Add(rend);
            }
        }
        foreach (var rend in toRestore)
            fadedObjects.Remove(rend);
    }

    private void OnDisable()
    {
        foreach (var rend in fadedObjects)
        {
            if (rend != null)
                rend.material = originalMaterial;
        }
        fadedObjects.Clear();
    }
}
