using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("안전 구역 청크")]
    [SerializeField]
    private GameObject startChunkPrefab;

    [SerializeField]
    private GameObject workbenchChunkPrefab;

    [SerializeField]
    private GameObject cauldronChunkPrefab;

    [SerializeField]
    private GameObject storageChunkPrefab;

    [SerializeField]
    private GameObject altarChunkPrefab;

    [SerializeField]
    private GameObject groundChunkPrefab;

    [SerializeField]
    private GameObject[] forestChunkPrefabs;

    [SerializeField]
    private int radius = 5;

    [SerializeField]
    private float chunkSize = 10f;

    private List<GameObject> forestChunks = new List<GameObject>();

    private void Start()
    {
        GenerateSafeZone();
        GenerateForest();
    }

    private void GenerateSafeZone()
    {
        var layout = new (Vector2Int, GameObject)[]
        {
            (new Vector2Int(-1, 1), workbenchChunkPrefab),
            (new Vector2Int(0, 1), storageChunkPrefab),
            (new Vector2Int(1, 1), groundChunkPrefab),
            (new Vector2Int(-1, 0), groundChunkPrefab),
            (new Vector2Int(0, 0), startChunkPrefab),
            (new Vector2Int(1, 0), altarChunkPrefab),
            (new Vector2Int(-1, -1), groundChunkPrefab),
            (new Vector2Int(0, -1), cauldronChunkPrefab),
            (new Vector2Int(1, -1), groundChunkPrefab),
        };

        foreach (var (gridPos, prefab) in layout)
        {
            Vector3 pos = new Vector3(gridPos.x * chunkSize, 0f, gridPos.y * chunkSize);
            Instantiate(prefab, pos, Quaternion.identity, transform);
        }
    }

    public void GenerateForest()
    {
        foreach (var chunk in forestChunks)
            Destroy(chunk);
        forestChunks.Clear();

        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                if (x >= -1 && x <= 1 && z >= -1 && z <= 1)
                    continue;

                Vector3 pos = new Vector3(x * chunkSize, 0f, z * chunkSize);
                GameObject prefab = forestChunkPrefabs[Random.Range(0, forestChunkPrefabs.Length)];
                forestChunks.Add(Instantiate(prefab, pos, Quaternion.identity, transform));
            }
        }
    }
}
