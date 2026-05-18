using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject startChunkPrefab;

    [SerializeField]
    private GameObject[] forestChunkPrefabs;

    [SerializeField]
    private int radius = 5;

    [SerializeField]
    private float chunkSize = 10f;

    private void Start()
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                Vector3 pos = new Vector3(x * chunkSize, 0f, z * chunkSize);
                GameObject prefab =
                    (x == 0 && z == 0)
                        ? startChunkPrefab
                        : forestChunkPrefabs[Random.Range(0, forestChunkPrefabs.Length)];
                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }
    }
}
