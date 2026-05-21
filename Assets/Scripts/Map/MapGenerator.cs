using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("야생 청크")]
    [SerializeField]
    private GameObject[] forestChunkPrefabs;

    [SerializeField]
    private int radius = 5;

    [SerializeField]
    private float chunkSize = 10f;

    private List<GameObject> forestChunks = new List<GameObject>();

    private void Start()
    {
        GenerateForest();
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
