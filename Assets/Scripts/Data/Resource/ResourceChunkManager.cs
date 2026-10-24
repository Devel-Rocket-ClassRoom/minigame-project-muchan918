using System.Collections.Generic;
using UnityEngine;

public class ResourceChunkManager : MonoBehaviour
{
    public const int CHUNK_SIZE = 10;
    [SerializeField, Min(0)]
    [Tooltip("플레이어 청크 기준 활성 반경(청크 단위). 활성 영역: (2R+1) × (2R+1)")]
    private int activeRadius = 2;

    private int lastActiveRadius = -1;

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

    private Dictionary<Vector2Int, ChunkData> chunks = new();
    private HashSet<Vector2Int> activeChunks = new();
    private Vector2Int lastPlayerChunk = new(int.MinValue, int.MinValue);

    private Transform resourceParent;
    private Transform playerTransform;

    private readonly HashSet<Vector2> destroyedPositions = new();
    public IReadOnlyCollection<Vector2> DestroyedPositions => destroyedPositions;

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
        chunks.Clear();
        activeChunks.Clear();
        lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        this.resourceParent = resourceParent;
    }

    // 스폰 정보만 등록 (Instantiate 안 함)
    public void RegisterSpawnInfo(Vector3 worldPosition, GameObject prefab)
    {
        var pos2D = new Vector2(worldPosition.x, worldPosition.z);
        if (destroyedPositions.Contains(pos2D))
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
        destroyedPositions.Add(pos2D);
        Debug.Log($"[Resource] 파괴 기록: {pos2D}, 총 {destroyedPositions.Count}개");

        var chunkCoord = WorldToChunk(pos);
        if (!chunks.TryGetValue(chunkCoord, out var chunk))
            return;

        chunk.SpawnedObjects.Remove(resourceObject);
    }

    // 맵 생성 완료 후 TileMapGenerator가 호출
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

        if (chunk.PendingSpawns.Count > 0)
        {
            foreach (var info in chunk.PendingSpawns)
            {
                var obj = Instantiate(
                    info.Prefab,
                    info.Position,
                    Quaternion.identity,
                    resourceParent
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
        if (!chunks.TryGetValue(chunkCoord, out var chunk))
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
        if (!chunks.TryGetValue(coord, out var chunk))
        {
            chunk = new ChunkData();
            chunks[coord] = chunk;
        }
        return chunk;
    }

    // 맵 재생성 시 호출
    public void Clear()
    {
        chunks.Clear();
        activeChunks.Clear();
        lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        playerTransform = null;
    }

    public void ClearDestroyedPositions()
    {
        destroyedPositions.Clear();
    }

    public void LoadDestroyedPositions(List<Vector2> positions)
    {
        destroyedPositions.Clear();
        foreach (var pos in positions)
            destroyedPositions.Add(pos);
        Debug.Log($"[Resource] 파괴 목록 로드: {destroyedPositions.Count}개");
    }
}
