using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGenerator : MonoBehaviour
{
    [System.Serializable]
    public class AnimalSpawnEntry
    {
        public GameObject prefab;

        [Min(0)]
        public int spawnCount;
    }

    [Header("Near Zone")]
    [SerializeField]
    private List<AnimalSpawnEntry> nearZone;

    [Header("Mid Zone")]
    [SerializeField]
    private List<AnimalSpawnEntry> midZone;

    [Header("Far Zone")]
    [SerializeField]
    private List<AnimalSpawnEntry> farZone;

    private TileMapGenerator tileMapGenerator;
    private Transform animalParent;

    private void Awake()
    {
        tileMapGenerator = GetComponent<TileMapGenerator>();
    }

    public void Generate()
    {
        if (animalParent != null)
            Destroy(animalParent.gameObject);

        animalParent = new GameObject("Animals").transform;
        animalParent.SetParent(transform);

        AnimalChunkManager.Instance.Initialize(animalParent);

        MapData mapData = tileMapGenerator.MapData;
        System.Random random = new System.Random(Random.Range(1, 999999));

        var nearTiles = GetAvailableTiles(mapData.NearTiles, mapData);
        var midTiles = GetAvailableTiles(mapData.MidTiles, mapData);
        var farTiles = GetAvailableTiles(mapData.FarTiles, mapData);

        Shuffle(nearTiles, random);
        Shuffle(midTiles, random);
        Shuffle(farTiles, random);

        RegisterZone(nearZone, nearTiles, mapData);
        RegisterZone(midZone, midTiles, mapData);
        RegisterZone(farZone, farTiles, mapData);
    }

    private List<Vector2Int> GetAvailableTiles(List<Vector2Int> tiles, MapData mapData)
    {
        var result = new List<Vector2Int>();
        foreach (var coord in tiles)
        {
            int x = coord.x + mapData.Width / 2;
            int y = coord.y + mapData.Height / 2;
            if (mapData.GetTile(x, y) != TileType.Resource)
                result.Add(coord);
        }
        return result;
    }

    private void RegisterZone(List<AnimalSpawnEntry> zone, List<Vector2Int> tiles, MapData mapData)
    {
        int tileIndex = 0;

        foreach (var entry in zone)
        {
            if (entry.prefab == null || entry.spawnCount <= 0)
                continue;

            int spawned = 0;

            while (spawned < entry.spawnCount)
            {
                if (tileIndex >= tiles.Count)
                {
                    Debug.LogWarning(
                        $"[AnimalGenerator] '{entry.prefab.name}' {entry.spawnCount}마리 목표 중 "
                            + $"{spawned}마리만 스폰됨 (타일 부족)"
                    );
                    break;
                }

                var coord = tiles[tileIndex++];

                AnimalChunkManager.Instance.RegisterSpawnInfo(
                    new Vector3(coord.x, 0f, coord.y),
                    entry.prefab
                );

                mapData.SetTile(coord, TileType.Resource);
                spawned++;
            }
        }
    }

    // private void SpawnZone(List<AnimalSpawnEntry> zone, List<Vector2Int> tiles, MapData mapData)
    // {
    //     int tileIndex = 0;

    //     foreach (var entry in zone)
    //     {
    //         if (entry.prefab == null || entry.spawnCount <= 0)
    //             continue;

    //         int spawned = 0;

    //         while (spawned < entry.spawnCount)
    //         {
    //             if (tileIndex >= tiles.Count)
    //             {
    //                 Debug.LogWarning(
    //                     $"[AnimalGenerator] '{entry.prefab.name}' {entry.spawnCount}마리 목표 중 "
    //                         + $"{spawned}마리만 스폰됨 (타일 부족)"
    //                 );
    //                 break;
    //             }

    //             var coord = tiles[tileIndex++];

    //             Instantiate(
    //                 entry.prefab,
    //                 new Vector3(coord.x, 0f, coord.y),
    //                 Quaternion.identity,
    //                 animalParent
    //             );

    //             mapData.SetTile(coord, TileType.Resource);
    //             spawned++;
    //         }
    //     }
    // }

    private static void Shuffle<T>(List<T> list, System.Random random)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
