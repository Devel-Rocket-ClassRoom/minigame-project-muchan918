using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour, IUpgradeable
{
    [System.Serializable]
    public class ResourceSpawnEntry
    {
        public GameObject prefab;

        [Range(0f, 1f)]
        public float spawnChance;
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
}
