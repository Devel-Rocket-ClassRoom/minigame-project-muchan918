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

    [SerializeField]
    private string leavesBoneName = "Leaves";

    [SerializeField]
    private string trunkBoneName = "Trunk";

    private Camera cam;
    private HashSet<GameObject> hiddenLeaves = new();
    private HashSet<Renderer> fadedRenderers = new();

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HashSet<GameObject> hitLeaves = new();
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
            Transform canopy = hit.collider.transform;

            // 나뭇잎 처리
            Transform leaves = canopy.Find(leavesBoneName);
            if (leaves != null)
            {
                hitLeaves.Add(leaves.gameObject);
                if (leaves.gameObject.activeSelf)
                {
                    leaves.gameObject.SetActive(false);
                    hiddenLeaves.Add(leaves.gameObject);
                }
            }

            // 줄기 처리
            Transform trunk = canopy.Find(trunkBoneName);
            if (trunk != null)
            {
                foreach (var rend in trunk.GetComponentsInChildren<Renderer>())
                {
                    hitRenderers.Add(rend);
                    if (!fadedRenderers.Contains(rend))
                    {
                        rend.material = transparentMaterial;
                        fadedRenderers.Add(rend);
                    }
                }
            }
        }

        // 나뭇잎 복원
        List<GameObject> toShow = new();
        foreach (var leaf in hiddenLeaves)
        {
            if (!hitLeaves.Contains(leaf))
            {
                if (leaf != null)
                    leaf.SetActive(true);
                toShow.Add(leaf);
            }
        }
        foreach (var leaf in toShow)
            hiddenLeaves.Remove(leaf);

        // 줄기 복원
        List<Renderer> toRestore = new();
        foreach (var rend in fadedRenderers)
        {
            if (!hitRenderers.Contains(rend))
            {
                if (rend != null)
                    rend.material = originalMaterial;
                toRestore.Add(rend);
            }
        }
        foreach (var rend in toRestore)
            fadedRenderers.Remove(rend);
    }

    private void OnDisable()
    {
        foreach (var leaf in hiddenLeaves)
            if (leaf != null)
                leaf.SetActive(true);
        hiddenLeaves.Clear();

        foreach (var rend in fadedRenderers)
            if (rend != null)
                rend.material = originalMaterial;
        fadedRenderers.Clear();
    }
}
