using System.Collections.Generic;
using UnityEngine;

public class ResourceChunkManager : MonoBehaviour
{
    public const int CHUNK_SIZE = 10;
    private const int ACTIVE_RADIUS = 2;

    public static ResourceChunkManager Instance { get; private set; }

    public class ResourceSpawnInfo
    {
        public GameObject Prefab;
        public Vector3 Position;
    }

    private class ChunkData
    {
        public List<ResourceSpawnInfo> PendingSpawns = new();
        public List<GameObject> SpawnedObjects = new();
    }

    private Dictionary<Vector2Int, ChunkData> _chunks = new();
    private HashSet<Vector2Int> _activeChunks = new();
    private Vector2Int _lastPlayerChunk = new(int.MinValue, int.MinValue);

    private Transform _resourceParent;
    private Transform _playerTransform;

    private readonly HashSet<Vector2> _destroyedPositions = new();
    public IReadOnlyCollection<Vector2> DestroyedPositions => _destroyedPositions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 맵 생성 시 ResourceGenerator가 호출
    public void Initialize(Transform resourceParent)
    {
        _chunks.Clear();
        _activeChunks.Clear();
        _lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        _resourceParent = resourceParent;
    }

    // 스폰 정보만 등록 (Instantiate 안 함)
    public void RegisterSpawnInfo(Vector3 worldPosition, GameObject prefab)
    {
        var pos2D = new Vector2(worldPosition.x, worldPosition.z);
        if (_destroyedPositions.Contains(pos2D))
        {
            return;
        }

        var chunkCoord = WorldToChunk(worldPosition);
        var chunk = GetOrCreateChunk(chunkCoord);
        chunk.PendingSpawns.Add(
            new ResourceSpawnInfo { Prefab = prefab, Position = worldPosition }
        );
    }

    // 자원 파괴 시 ResourceObject가 호출
    public void UnregisterResource(GameObject resourceObject)
    {
        var pos = resourceObject.transform.position;
        var pos2D = new Vector2(pos.x, pos.z);
        _destroyedPositions.Add(pos2D);
        Debug.Log($"[Resource] 파괴 기록: {pos2D}, 총 {_destroyedPositions.Count}개");

        var chunkCoord = WorldToChunk(pos);
        if (!_chunks.TryGetValue(chunkCoord, out var chunk))
            return;

        chunk.SpawnedObjects.Remove(resourceObject);
    }

    // 맵 생성 완료 후 TileMapGenerator가 호출
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

        if (chunk.PendingSpawns.Count > 0)
        {
            foreach (var info in chunk.PendingSpawns)
            {
                var obj = Instantiate(
                    info.Prefab,
                    info.Position,
                    Quaternion.identity,
                    _resourceParent
                );
                chunk.SpawnedObjects.Add(obj);
            }
            chunk.PendingSpawns.Clear();
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

    private Vector2Int WorldToChunk(Vector3 worldPos)
    {
        int chunkX = Mathf.FloorToInt(worldPos.x / CHUNK_SIZE);
        int chunkZ = Mathf.FloorToInt(worldPos.z / CHUNK_SIZE);
        return new Vector2Int(chunkX, chunkZ);
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

    // 맵 재생성 시 호출
    public void Clear()
    {
        _chunks.Clear();
        _activeChunks.Clear();
        _lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        _playerTransform = null;
    }

    public void ClearDestroyedPositions()
    {
        _destroyedPositions.Clear();
    }

    public void LoadDestroyedPositions(List<Vector2> positions)
    {
        _destroyedPositions.Clear();
        foreach (var pos in positions)
            _destroyedPositions.Add(pos);
        Debug.Log($"[Resource] 파괴 목록 로드: {_destroyedPositions.Count}개");
    }
}
