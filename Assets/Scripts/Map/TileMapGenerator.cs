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

    [Header("Zone 경계 (중심에서 거리 기준)")]
    [SerializeField]
    private float nearZoneRadius = 50f;

    [SerializeField]
    private float midZoneRadius = 100f;

    private MapData _mapData;
    private ResourceGenerator resourceGenerator;
    private AnimalGenerator animalGenerator;
    private Transform _tileParent;

    public MapData MapData => _mapData;
    public int CurrentSeed { get; private set; }

    private void Awake()
    {
        resourceGenerator = GetComponent<ResourceGenerator>();
        animalGenerator = GetComponent<AnimalGenerator>();
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
        LoadingUI.Instance.ShowInit();

        CurrentSeed = seed == 0 ? Random.Range(1, 999999) : seed;
        _mapData = new MapData(mapWidth, mapHeight, CurrentSeed, nearZoneRadius, midZoneRadius);

        _tileParent = new GameObject("Ground").transform;
        _tileParent.SetParent(transform);

        TileChunkManager.Instance.Initialize(_mapData, _tileParent, CurrentSeed);

        StartCoroutine(GenerateSequence());
    }

    public void GenerateMap(int forcedSeed)
    {
        seed = forcedSeed;
        GenerateMap();
        seed = 0;
    }

    private IEnumerator GenerateSequence()
    {
        LoadingUI.Instance.SetProgress(0.3f);
        yield return null;
        Debug.Log("맵 생성 완료");

        yield return StartCoroutine(resourceGenerator.SpawnCoroutine());
        LoadingUI.Instance.SetProgress(0.6f);
        yield return null;
        Debug.Log("자원 생성 완료");

        animalGenerator.Generate();
        LoadingUI.Instance.SetProgress(0.9f);
        yield return null;
        Debug.Log("동물 생성 완료");

        TileChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);
        ResourceChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);
        AnimalChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);

        LoadingUI.Instance.SetProgress(1f);
        yield return new WaitForSecondsRealtime(2f);
        LoadingUI.Instance.Hide();
        SoundManager.Instance.PlayMainBgm();
        GamePause.Resume();
    }

    private void ClearMap()
    {
        if (_tileParent != null)
            Destroy(_tileParent.gameObject);

        TileChunkManager.Instance.Clear();
        ResourceChunkManager.Instance.Clear();
        AnimalChunkManager.Instance.Clear();
    }
}
