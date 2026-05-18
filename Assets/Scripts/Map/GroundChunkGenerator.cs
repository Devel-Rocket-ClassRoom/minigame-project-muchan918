using UnityEngine;

public class GroundChunkGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private int chunkSize = 10;

    [ContextMenu("Generate")]
    private void Generate()
    {
        int half = chunkSize / 2;
        for (int x = -half; x < half; x++)
        {
            for (int z = -half; z < half; z++)
            {
                Instantiate(
                    tilePrefab,
                    transform.position + new Vector3(x, 0f, z),
                    Quaternion.identity,
                    transform
                );
            }
        }
    }
}
