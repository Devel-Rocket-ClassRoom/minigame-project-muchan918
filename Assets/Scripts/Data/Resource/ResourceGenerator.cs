using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [System.Serializable]
    public class ResourceSpawnEntry
    {
        public GameObject prefab;

        [Range(0f, 1f)]
        public float spawnChance;
    }

    [Header("Near Zone (희귀한 것 먼저)")]
    [SerializeField]
    private List<ResourceSpawnEntry> nearZone;

    [Header("Mid Zone (희귀한 것 먼저)")]
    [SerializeField]
    private List<ResourceSpawnEntry> midZone;

    [Header("Far Zone (희귀한 것 먼저)")]
    [SerializeField]
    private List<ResourceSpawnEntry> farZone;

    private TileMapGenerator tileMapGenerator;

    private Transform resourceParent;

    private void Awake()
    {
        tileMapGenerator = GetComponent<TileMapGenerator>();
    }

    public void Generate()
    {
        if (resourceParent != null)
            Destroy(resourceParent.gameObject);

        resourceParent = new GameObject("Resources").transform;
        resourceParent.SetParent(transform);

        ResourceChunkManager.Instance.Initialize(resourceParent);
    }

    public IEnumerator SpawnCoroutine()
    {
        Generate();

        MapData mapData = tileMapGenerator.MapData;
        System.Random random = new System.Random(Random.Range(1, 999999));

        yield return RegisterZone(mapData.NearTiles, nearZone, mapData, random);
        yield return RegisterZone(mapData.MidTiles, midZone, mapData, random);
        yield return RegisterZone(mapData.FarTiles, farZone, mapData, random);
    }

    private IEnumerator RegisterZone(
        List<Vector2Int> tiles,
        List<ResourceSpawnEntry> zone,
        MapData mapData,
        System.Random random
    )
    {
        int count = 0;
        const int registerPerFrame = 500;

        foreach (var coord in tiles)
        {
            foreach (var entry in zone)
            {
                if (entry.prefab == null)
                    continue;
                if (random.NextDouble() > entry.spawnChance)
                    continue;

                ResourceChunkManager.Instance.RegisterSpawnInfo(
                    new Vector3(coord.x, 1f, coord.y),
                    entry.prefab
                );

                mapData.SetTile(coord, TileType.Resource);
                break;
            }

            count++;
            if (count >= registerPerFrame)
            {
                count = 0;
                yield return null;
            }
        }
    }

    // private IEnumerator SpawnZone(
    //     List<Vector2Int> tiles,
    //     List<ResourceSpawnEntry> zone,
    //     MapData mapData,
    //     System.Random random
    // )
    // {
    //     int count = 0;
    //     const int spawnPerFrame = 300;

    //     foreach (var coord in tiles)
    //     {
    //         foreach (var entry in zone)
    //         {
    //             if (entry.prefab == null)
    //                 continue;
    //             if (random.NextDouble() > entry.spawnChance)
    //                 continue;

    //             Instantiate(
    //                 entry.prefab,
    //                 new Vector3(coord.x, 1f, coord.y),
    //                 Quaternion.identity,
    //                 resourceParent
    //             );

    //             mapData.SetTile(coord, TileType.Resource);
    //             break;
    //         }

    //         count++;
    //         if (count >= spawnPerFrame)
    //         {
    //             count = 0;
    //             yield return null;
    //         }
    //     }
    // }
}
