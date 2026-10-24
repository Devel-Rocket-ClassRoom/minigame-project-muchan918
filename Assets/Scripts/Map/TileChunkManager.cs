using System.Collections.Generic;
using UnityEngine;

public class TileChunkManager : MonoBehaviour
{
    public const int CHUNK_SIZE = 10;
    [SerializeField, Min(0)]
    [Tooltip("플레이어 청크 기준 활성 반경(청크 단위). 활성 영역: (2R+1) × (2R+1)")]
    private int activeRadius = 3;

    private int lastActiveRadius = -1;

    public static TileChunkManager Instance { get; private set; }

    [SerializeField]
    private GameObject[] groundTilePrefabs;

    [SerializeField]
    private GameObject[] grassGroundTilePrefabs;

    [SerializeField]
    private GameObject waterTilePrefab;

    [SerializeField]
    private GameObject borderTilePrefab;

    private class ChunkData
    {
        public List<(Vector3 pos, TileType type)> PendingTiles = new();
        public List<GameObject> SpawnedObjects = new();
    }

    private Dictionary<Vector2Int, ChunkData> chunks = new();
    private HashSet<Vector2Int> activeChunks = new();
    private Vector2Int lastPlayerChunk = new(int.MinValue, int.MinValue);

    private Transform tileParent;
    private Transform playerTransform;
    private System.Random random;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Initialize(MapData mapData, Transform tileParent, int seed, int borderSize = 30)
    {
        Clear();
        this.tileParent = tileParent;
        random = new System.Random(seed);

        int halfWidth = mapData.Width / 2;
        int halfHeight = mapData.Height / 2;
        int baseHalf = 15;

        for (int y = 0; y < mapData.Height; y++)
            for (int x = 0; x < mapData.Width; x++)
            {
                int wx = x - halfWidth;
                int wy = y - halfHeight;

                if (wx >= -baseHalf && wx < baseHalf && wy >= -baseHalf && wy < baseHalf)
                    continue;

                TileType type = mapData.GetTile(x, y);
                var pos = new Vector3(wx, 0f, wy);
                var chunkCoord = WorldToChunk(pos);

                var chunk = GetOrCreateChunk(chunkCoord);
                chunk.PendingTiles.Add((pos, type));
            }

        // 외곽 Water 타일
        for (int wx = -(halfWidth + borderSize); wx < halfWidth + borderSize; wx++)
            for (int wy = -(halfHeight + borderSize); wy < halfHeight + borderSize; wy++)
            {
                bool insideMap =
                    wx >= -halfWidth && wx < halfWidth && wy >= -halfHeight && wy < halfHeight;
                if (insideMap)
                    continue;

                var pos = new Vector3(wx, 0f, wy);
                GetOrCreateChunk(WorldToChunk(pos)).PendingTiles.Add((pos, TileType.Border));
            }
    }

    public void StartTracking(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        UpdateChunks();
    }

    private void Update()
    {
        if (playerTransform == null)
            return;
        activeRadius = Mathf.Max(0, activeRadius);
        if (WorldToChunk(playerTransform.position) == lastPlayerChunk
            && activeRadius == lastActiveRadius)
            return;
        UpdateChunks();
    }

    private void UpdateChunks()
    {
        var playerChunk = WorldToChunk(playerTransform.position);
        lastPlayerChunk = playerChunk;
        activeRadius = Mathf.Max(0, activeRadius);
        lastActiveRadius = activeRadius;

        var newActiveChunks = new HashSet<Vector2Int>();
        for (int dy = -activeRadius; dy <= activeRadius; dy++)
            for (int dx = -activeRadius; dx <= activeRadius; dx++)
                newActiveChunks.Add(new Vector2Int(playerChunk.x + dx, playerChunk.y + dy));

        foreach (var coord in activeChunks)
            if (!newActiveChunks.Contains(coord))
                DeactivateChunk(coord);

        foreach (var coord in newActiveChunks)
            if (!activeChunks.Contains(coord))
                ActivateChunk(coord);

        activeChunks = newActiveChunks;
    }

    private void ActivateChunk(Vector2Int chunkCoord)
    {
        if (!chunks.TryGetValue(chunkCoord, out var chunk))
            return;

        if (chunk.PendingTiles.Count > 0)
        {
            foreach (var (pos, type) in chunk.PendingTiles)
            {
                var prefab = GetPrefab(type);
                if (prefab == null)
                    continue;
                var obj = Instantiate(prefab, pos, Quaternion.identity, tileParent);
                chunk.SpawnedObjects.Add(obj);
            }
            chunk.PendingTiles.Clear();
        }

        foreach (var obj in chunk.SpawnedObjects)
            if (obj != null)
                obj.SetActive(true);
    }

    private void DeactivateChunk(Vector2Int chunkCoord)
    {
        if (!chunks.TryGetValue(chunkCoord, out var chunk))
            return;
        foreach (var obj in chunk.SpawnedObjects)
            if (obj != null)
                obj.SetActive(false);
    }

    private GameObject GetPrefab(TileType type)
    {
        return type switch
        {
            TileType.Water => waterTilePrefab,
            TileType.Border => borderTilePrefab,
            TileType.GrassGround => grassGroundTilePrefabs[
                random.Next(0, grassGroundTilePrefabs.Length)
            ],
            _ => groundTilePrefabs[random.Next(0, groundTilePrefabs.Length)],
        };
    }

    private Vector2Int WorldToChunk(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / CHUNK_SIZE),
            Mathf.FloorToInt(worldPos.z / CHUNK_SIZE)
        );
    }

    private ChunkData GetOrCreateChunk(Vector2Int coord)
    {
        if (!chunks.TryGetValue(coord, out var chunk))
        {
            chunk = new ChunkData();
            chunks[coord] = chunk;
        }
        return chunk;
    }

    public void Clear()
    {
        chunks.Clear();
        activeChunks.Clear();
        lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        playerTransform = null;
    }
}
