using System.Collections;
using UnityEngine;

public class TutorialMapGenerator : MonoBehaviour
{
    [Header("맵 설정")]
    [SerializeField]
    private int mapWidth = 40;

    [SerializeField]
    private int mapHeight = 40;

    [SerializeField]
    private int seed = 0;

    [SerializeField]
    private float nearZoneRadius = 10f;

    [SerializeField]
    private float midZoneRadius = 20f;

    private MapData _mapData;
    private Transform _tileParent;

    public MapData MapData => _mapData;
    public int CurrentSeed { get; private set; }

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
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

    private IEnumerator GenerateSequence()
    {
        LoadingUI.Instance.SetProgress(0.5f);
        yield return null;

        TileChunkManager.Instance.StartTracking(PlayerSpawner.Instance.PlayerTransform);

        LoadingUI.Instance.SetProgress(1f);
        yield return new WaitForSecondsRealtime(2f);
        LoadingUI.Instance.Hide();
        SoundManager.Instance.PlayMainBgm();
        GamePause.Resume();

        TutorialSceneManager.Instance.OnMapReady(); // 추가
    }

    private void ClearMap()
    {
        if (_tileParent != null)
            Destroy(_tileParent.gameObject);
        TileChunkManager.Instance.Clear();
    }
}
