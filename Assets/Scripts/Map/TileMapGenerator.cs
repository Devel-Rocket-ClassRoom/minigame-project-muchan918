using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMapGenerator : MonoBehaviour
{
    [Header("맵 설정")]
    [SerializeField]
    private int mapWidth = 300;

    [SerializeField]
    private int mapHeight = 300;

    [SerializeField]
    private int seed = 0;

    [Header("타일 프리팹")]
    [SerializeField]
    private GameObject[] groundTilePrefabs;

    [SerializeField]
    private GameObject[] grassGroundTilePrefabs;

    [SerializeField]
    private GameObject waterTilePrefab;

    private MapData _mapData;
    private Transform _groundParent;

    public MapData MapData => _mapData;

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
        _mapData = new MapData(mapWidth, mapHeight, usedSeed);

        _groundParent = new GameObject("Ground").transform;
        _groundParent.SetParent(transform);

        StartCoroutine(SpawnTilesCoroutine());
    }

    private IEnumerator SpawnTilesCoroutine()
    {
        int tilesPerFrame = 500;
        int count = 0;
        int halfWidth = mapWidth / 2;
        int halfHeight = mapHeight / 2;
        int baseHalf = 15; // 30x30의 절반

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // 중앙 30x30 스킵
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

                Vector3 pos = new Vector3(x - halfWidth, 0f, y - halfHeight);
                Instantiate(prefab, pos, Quaternion.identity, _groundParent);

                count++;
                if (count >= tilesPerFrame)
                {
                    count = 0;
                    yield return null;
                }
            }
        }

        Debug.Log("맵 생성 완료");
        GamePause.Resume();
    }

    private void ClearMap()
    {
        if (_groundParent != null)
            Destroy(_groundParent.gameObject);
    }
}
