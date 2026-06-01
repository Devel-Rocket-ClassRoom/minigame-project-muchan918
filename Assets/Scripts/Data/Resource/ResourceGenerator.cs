using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour, IUpgradeable
{
    [System.Serializable]
    public class ResourceSpawnEntry
    {
        public GameObject prefab;
        public string resourceId;
        public string resourceType;

        [Range(0f, 1f)]
        public float spawnChance;

        public bool useSpread;
        public int spreadSeedCount = 3;
        public int spreadDepth = 5;

        [Range(0f, 1f)]
        public float spreadChance = 0.55f;
    }

    [System.Serializable]
    public class ResourceUpgradeLevel
    {
        public List<ResourceSpawnEntry> nearZone;
        public List<ResourceSpawnEntry> midZone;
        public List<ResourceSpawnEntry> farZone;
    }

    [SerializeField]
    private List<ResourceUpgradeLevel> spawnEntriesByLevel;

    private List<ResourceSpawnEntry> nearZone;
    private List<ResourceSpawnEntry> midZone;
    private List<ResourceSpawnEntry> farZone;

    private TileMapGenerator tileMapGenerator;
    private Transform resourceParent;

    public int Level { get; private set; }

    private void Awake()
    {
        tileMapGenerator = GetComponent<TileMapGenerator>();

        if (spawnEntriesByLevel.Count > 0)
        {
            var entries = spawnEntriesByLevel[0];
            nearZone = entries.nearZone;
            midZone = entries.midZone;
            farZone = entries.farZone;
        }
    }

    public void Upgrade()
    {
        if (Level >= spawnEntriesByLevel.Count - 1)
            return;

        Level++;

        var entries = spawnEntriesByLevel[Level];
        nearZone = entries.nearZone;
        midZone = entries.midZone;
        farZone = entries.farZone;
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
        System.Random random = new System.Random(tileMapGenerator.CurrentSeed);

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
        const int perFrame = 500;

        // Spread 먼저 배치
        foreach (var entry in zone)
        {
            if (!entry.useSpread)
                continue;

            for (int i = 0; i < entry.spreadSeedCount; i++)
            {
                // 시드 위치 랜덤 선정 (zone 타일 중에서)
                if (tiles.Count == 0)
                    break;
                Vector2Int seed = tiles[random.Next(0, tiles.Count)];

                SpreadResource(seed, entry, mapData, random);
            }

            count++;
            if (count >= perFrame)
            {
                count = 0;
                yield return null;
            }
        }

        foreach (var coord in tiles)
        {
            if (mapData.GetTileWorld(coord) == TileType.Ground)
            {
                foreach (var entry in zone)
                {
                    if (entry.useSpread)
                        continue;
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
            }

            count++;
            if (count >= perFrame)
            {
                count = 0;
                yield return null;
            }
        }
    }

    private void SpreadResource(
        Vector2Int origin,
        ResourceSpawnEntry entry,
        MapData mapData,
        System.Random random
    )
    {
        SpreadFrom(origin, entry, entry.spreadDepth, mapData, random);
    }

    private void SpreadFrom(
        Vector2Int coord,
        ResourceSpawnEntry entry,
        int depth,
        MapData mapData,
        System.Random random
    )
    {
        if (depth <= 0)
            return;

        var tileType = mapData.GetTileWorld(coord);
        if (tileType != TileType.Ground)
            return;

        ResourceChunkManager.Instance.RegisterSpawnInfo(
            new Vector3(coord.x, 1f, coord.y),
            entry.prefab
        );
        mapData.SetTile(coord, TileType.Resource);

        if (random.NextDouble() < entry.spreadChance)
            SpreadFrom(coord + Vector2Int.right, entry, depth - 1, mapData, random);
        if (random.NextDouble() < entry.spreadChance)
            SpreadFrom(coord + Vector2Int.left, entry, depth - 1, mapData, random);
        if (random.NextDouble() < entry.spreadChance)
            SpreadFrom(coord + Vector2Int.up, entry, depth - 1, mapData, random);
        if (random.NextDouble() < entry.spreadChance)
            SpreadFrom(coord + Vector2Int.down, entry, depth - 1, mapData, random);
    }
}
