using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMapGenerator : MonoBehaviour
{
    public static event System.Action OnMapLoadComplete;

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
    private bool isClear = false;

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

    public void GenerateMap(bool clear = false)
    {
        if (GamePause.IsPaused)
            return;
        isClear = clear;
        ClearMap();
        GamePause.Pause();

        if (!isClear)
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
        if (isClear)
            LoadingUI.Instance.SetLoadingText("맵 재생성중");
        yield return null;

        yield return StartCoroutine(resourceGenerator.SpawnCoroutine());
        LoadingUI.Instance.SetProgress(0.6f);
        if (isClear)
            LoadingUI.Instance.SetLoadingText("자원 재생성중");
        yield return null;

        animalGenerator.Generate();
        LoadingUI.Instance.SetProgress(0.9f);
        if (isClear)
            LoadingUI.Instance.SetLoadingText("동물 재생성중");
        yield return null;

        TileChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);
        ResourceChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);
        AnimalChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);

        LoadingUI.Instance.SetProgress(1f);

        if (!isClear)
        {
            yield return new WaitForSecondsRealtime(2f);
            LoadingUI.Instance.Hide();
            SoundManager.Instance.PlayMainBgm();
            GamePause.Resume();
            OnMapLoadComplete?.Invoke();
        }
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
