using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMapGenerator : MonoBehaviour
{
    [Header("맵 설정")]
    [SerializeField]
    private int mapWidth = 200;

    [SerializeField]
    private int mapHeight = 200;

    [SerializeField]
    private int seed = 0;

    [Header("Zone 경계 (중심에서 거리 기준)")]
    [SerializeField]
    private float nearZoneRadius = 50f;

    [SerializeField]
    private float midZoneRadius = 100f;

    [Header("타일 프리팹")]
    [SerializeField]
    private GameObject[] groundTilePrefabs;

    [SerializeField]
    private GameObject[] grassGroundTilePrefabs;

    [SerializeField]
    private GameObject waterTilePrefab;

    private MapData _mapData;
    private Transform _groundParent;
    private ResourceGenerator resourceGenerator;
    private AnimalGenerator animalGenerator;

    public MapData MapData => _mapData;

    private void Awake()
    {
        resourceGenerator = GetComponent<ResourceGenerator>();
        animalGenerator = GetComponent<AnimalGenerator>();
    }

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
            GenerateMap();
    }

    public void GenerateMap()
    {
        if (GamePause.IsPaused)
            return;
        ClearMap();
        GamePause.Pause();

        int usedSeed = seed == 0 ? Random.Range(1, 999999) : seed;
        _mapData = new MapData(mapWidth, mapHeight, usedSeed, nearZoneRadius, midZoneRadius);

        _groundParent = new GameObject("Ground").transform;
        _groundParent.SetParent(transform);

        StartCoroutine(GenerateSequence());
    }

    private IEnumerator GenerateSequence()
    {
        yield return StartCoroutine(SpawnTilesCoroutine());
        Debug.Log("맵 생성 완료");

        yield return StartCoroutine(resourceGenerator.SpawnCoroutine());
        Debug.Log("자원 생성 완료");

        animalGenerator.Generate();
        Debug.Log("동물 생성 완료");

        ResourceChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);
        AnimalChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);

        GamePause.Resume();
    }

    private IEnumerator SpawnTilesCoroutine()
    {
        int count = 0;
        int tilesPerFrame = 500;
        int halfWidth = mapWidth / 2;
        int halfHeight = mapHeight / 2;
        int baseHalf = 15;

        for (int y = 0; y < mapHeight; y++)
        for (int x = 0; x < mapWidth; x++)
        {
            int wx = x - halfWidth;
            int wy = y - halfHeight;
            if (wx >= -baseHalf && wx < baseHalf && wy >= -baseHalf && wy < baseHalf)
                continue;

            TileType type = _mapData.GetTile(x, y);

            GameObject prefab;
            if (type == TileType.Water)
                prefab = waterTilePrefab;
            else if (type == TileType.GrassGround)
                prefab = grassGroundTilePrefabs[Random.Range(0, grassGroundTilePrefabs.Length)];
            else
                prefab = groundTilePrefabs[Random.Range(0, groundTilePrefabs.Length)];

            Instantiate(prefab, new Vector3(wx, 0f, wy), Quaternion.identity, _groundParent);

            count++;
            if (count >= tilesPerFrame)
            {
                count = 0;
                yield return null;
            }
        }
    }

    private void ClearMap()
    {
        if (_groundParent != null)
            Destroy(_groundParent.gameObject);

        ResourceChunkManager.Instance.Clear();
        AnimalChunkManager.Instance.Clear();
    }
}
