using System.Collections.Generic;
using UnityEngine;

public class TileChunkManager : MonoBehaviour
{
    public const int CHUNK_SIZE = 10;
    private const int ACTIVE_RADIUS = 3;

    public static TileChunkManager Instance { get; private set; }

    [SerializeField]
    private GameObject[] groundTilePrefabs;

    [SerializeField]
    private GameObject[] grassGroundTilePrefabs;

    [SerializeField]
    private GameObject waterTilePrefab;

    private class ChunkData
    {
        public List<(Vector3 pos, TileType type)> PendingTiles = new();
        public List<GameObject> SpawnedObjects = new();
    }

    private Dictionary<Vector2Int, ChunkData> _chunks = new();
    private HashSet<Vector2Int> _activeChunks = new();
    private Vector2Int _lastPlayerChunk = new(int.MinValue, int.MinValue);

    private Transform _tileParent;
    private Transform _playerTransform;
    private System.Random _random;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Initialize(MapData mapData, Transform tileParent, int seed)
    {
        Clear();
        _tileParent = tileParent;
        _random = new System.Random(seed);

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
    }

    public void StartTracking(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        UpdateChunks();
    }

    private void Update()
    {
        if (_playerTransform == null)
            return;
        if (WorldToChunk(_playerTransform.position) == _lastPlayerChunk)
            return;
        UpdateChunks();
    }

    private void UpdateChunks()
    {
        var playerChunk = WorldToChunk(_playerTransform.position);
        _lastPlayerChunk = playerChunk;

        var newActiveChunks = new HashSet<Vector2Int>();
        for (int dy = -ACTIVE_RADIUS; dy <= ACTIVE_RADIUS; dy++)
        for (int dx = -ACTIVE_RADIUS; dx <= ACTIVE_RADIUS; dx++)
            newActiveChunks.Add(new Vector2Int(playerChunk.x + dx, playerChunk.y + dy));

        foreach (var coord in _activeChunks)
            if (!newActiveChunks.Contains(coord))
                DeactivateChunk(coord);

        foreach (var coord in newActiveChunks)
            if (!_activeChunks.Contains(coord))
                ActivateChunk(coord);

        _activeChunks = newActiveChunks;
    }

    private void ActivateChunk(Vector2Int chunkCoord)
    {
        if (!_chunks.TryGetValue(chunkCoord, out var chunk))
            return;

        if (chunk.PendingTiles.Count > 0)
        {
            foreach (var (pos, type) in chunk.PendingTiles)
            {
                var prefab = GetPrefab(type);
                if (prefab == null)
                    continue;
                var obj = Instantiate(prefab, pos, Quaternion.identity, _tileParent);
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
        if (!_chunks.TryGetValue(chunkCoord, out var chunk))
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
            TileType.GrassGround => grassGroundTilePrefabs[
                _random.Next(0, grassGroundTilePrefabs.Length)
            ],
            _ => groundTilePrefabs[_random.Next(0, groundTilePrefabs.Length)],
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
        if (!_chunks.TryGetValue(coord, out var chunk))
        {
            chunk = new ChunkData();
            _chunks[coord] = chunk;
        }
        return chunk;
    }

    public void Clear()
    {
        _chunks.Clear();
        _activeChunks.Clear();
        _lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        _playerTransform = null;
    }
}
