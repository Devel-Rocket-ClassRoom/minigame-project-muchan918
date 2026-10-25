using UnityEngine;

public class OcclusionTarget : MonoBehaviour
{
    [SerializeField] private GameObject[] canopies;
    [SerializeField] private Renderer[] trunks;

    private bool isOccluded;

    public void SetOccluded(bool occluded, Material material)
    {
        if (isOccluded == occluded) return;

        isOccluded = occluded;

        foreach (GameObject canopy in canopies)
        {
            if (canopy != null)
                canopy.SetActive(!occluded);
        }

        foreach (Renderer trunk in trunks)
        {
            if (trunk != null)
                trunk.material = material;
        }
    }
}
