using System.Collections.Generic;
using UnityEngine;

public class AnimalChunkManager : MonoBehaviour
{
    public const int CHUNK_SIZE = 10;
    private const int ACTIVE_RADIUS = 2;

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

    private Dictionary<Vector2Int, ChunkData> _chunks = new();
    private HashSet<Vector2Int> _activeChunks = new();
    private Vector2Int _lastPlayerChunk = new(int.MinValue, int.MinValue);

    private Transform _animalParent;
    private Transform _playerTransform;

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
        _chunks.Clear();
        _activeChunks.Clear();
        _lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);
        _animalParent = animalParent;
    }

    // 스폰 정보만 등록 (Instantiate 안 함)
    public void RegisterSpawnInfo(Vector3 worldPosition, GameObject prefab)
    {
        var chunkCoord = WorldToChunk(worldPosition);
        var chunk = GetOrCreateChunk(chunkCoord);
        chunk.PendingSpawns.Add(new AnimalSpawnInfo { Prefab = prefab, Position = worldPosition });
    }

    // 동물 파괴 시 Animal.Die()가 호출
    public void UnregisterAnimal(Animal animal)
    {
        var chunkCoord = WorldToChunk(animal.transform.position);
        if (!_chunks.TryGetValue(chunkCoord, out var chunk))
            return;

        chunk.SpawnedAnimals.Remove(animal);
    }

    // 동물이 청크를 이동했을 때 Animal이 호출
    public void UpdateAnimalChunk(Animal animal, Vector2Int oldChunk, Vector2Int newChunk)
    {
        if (_chunks.TryGetValue(oldChunk, out var old))
            old.SpawnedAnimals.Remove(animal);

        var next = GetOrCreateChunk(newChunk);
        next.SpawnedAnimals.Add(animal);

        // 새 청크가 활성 범위 밖이면 바로 비활성화
        if (!_activeChunks.Contains(newChunk))
        {
            animal.OnDeactivate();
            animal.gameObject.SetActive(false);
        }
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
                    _animalParent
                );
                var animal = obj.GetComponent<Animal>();
                if (animal != null)
                    chunk.SpawnedAnimals.Add(animal);
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
        if (!_chunks.TryGetValue(chunkCoord, out var chunk))
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
}
