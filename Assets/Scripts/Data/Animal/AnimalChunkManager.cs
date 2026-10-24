using System.Collections.Generic;
using UnityEngine;

public class AnimalChunkManager : MonoBehaviour
{
    public const int CHUNK_SIZE = 10;
    [SerializeField, Min(0)]
    [Tooltip("플레이어 청크 기준 활성 반경(청크 단위). 활성 영역: (2R+1) × (2R+1)")]
    private int activeRadius = 2;

    private int lastActiveRadius = -1;

    public static AnimalChunkManager Instance { get; private set; }

    public class AnimalSpawnInfo
    {
        public GameObject Prefab;
        public Vector3 Position;
    }

    private class ChunkData
    {
        public List<AnimalSpawnInfo> PendingSpawns = new();
        public List<Animal> SpawnedAnimals = new();
    }

    private Dictionary<Vector2Int, ChunkData> chunks = new();
    private HashSet<Vector2Int> activeChunks = new();
    private Vector2Int lastPlayerChunk = new(int.MinValue, int.MinValue);

    private Transform animalParent;
    private Transform playerTransform;

    private readonly HashSet<Vector2> deadSpawnPositions = new();
    public IReadOnlyCollection<Vector2> DeadSpawnPositions => deadSpawnPositions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 맵 생성 시 AnimalGenerator가 호출
    public void Initialize(Transform animalParent)
    {
        chunks.Clear();
        activeChunks.Clear();
        lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        this.animalParent = animalParent;
    }

    // 스폰 정보만 등록 (Instantiate 안 함)
    public void RegisterSpawnInfo(Vector3 worldPosition, GameObject prefab)
    {
        var pos2D = new Vector2(worldPosition.x, worldPosition.z);
        if (deadSpawnPositions.Contains(pos2D))
            return;

        var chunkCoord = WorldToChunk(worldPosition);
        var chunk = GetOrCreateChunk(chunkCoord);
        chunk.PendingSpawns.Add(new AnimalSpawnInfo { Prefab = prefab, Position = worldPosition });
    }

    // 동물 파괴 시 Animal.Die()가 호출
    public void UnregisterAnimal(Animal animal)
    {
        deadSpawnPositions.Add(new Vector2(animal.SpawnPosition.x, animal.SpawnPosition.z));

        var chunkCoord = WorldToChunk(animal.transform.position);
        if (!chunks.TryGetValue(chunkCoord, out var chunk))
            return;

        chunk.SpawnedAnimals.Remove(animal);
    }

    // 동물이 청크를 이동했을 때 Animal이 호출
    public void UpdateAnimalChunk(Animal animal, Vector2Int oldChunk, Vector2Int newChunk)
    {
        if (chunks.TryGetValue(oldChunk, out var old))
            old.SpawnedAnimals.Remove(animal);

        var next = GetOrCreateChunk(newChunk);
        next.SpawnedAnimals.Add(animal);

        // 새 청크가 활성 범위 밖이면 바로 비활성화
        if (!activeChunks.Contains(newChunk))
        {
            animal.OnDeactivate();
            animal.gameObject.SetActive(false);
        }
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
                    animalParent
                );
                var animal = obj.GetComponent<Animal>();
                if (animal != null)
                {
                    animal.SpawnPosition = info.Position;
                    chunk.SpawnedAnimals.Add(animal);
                }
            }
            chunk.PendingSpawns.Clear();
        }

        foreach (var animal in chunk.SpawnedAnimals)
        {
            if (animal != null)
            {
                bool needsActivate = !animal.gameObject.activeSelf;
                animal.gameObject.SetActive(true);
                if (needsActivate)
                    animal.OnActivate();
            }
        }
    }

    private void DeactivateChunk(Vector2Int chunkCoord)
    {
        if (!chunks.TryGetValue(chunkCoord, out var chunk))
            return;

        foreach (var animal in chunk.SpawnedAnimals)
        {
            if (animal != null)
            {
                animal.OnDeactivate();
                animal.gameObject.SetActive(false);
            }
        }
    }

    public Vector2Int WorldToChunk(Vector3 worldPos)
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

    public void ClearDeadPositions()
    {
        deadSpawnPositions.Clear();
    }

    public void LoadDeadPositions(List<Vector2> positions)
    {
        deadSpawnPositions.Clear();
        foreach (var pos in positions)
            deadSpawnPositions.Add(pos);
        Debug.Log($"[Animal] 죽은 위치 로드: {deadSpawnPositions.Count}개");
    }

    public void ResetLivingAnimals()
    {
        foreach (var chunk in chunks.Values)
            foreach (var animal in chunk.SpawnedAnimals)
                if (animal != null)
                    animal.ResetToSpawn();
    }

    public void RegisterAnimal(Animal animal)
    {
        var chunkCoord = WorldToChunk(animal.transform.position);
        var chunk = GetOrCreateChunk(chunkCoord);
        chunk.SpawnedAnimals.Add(animal);
    }
}
