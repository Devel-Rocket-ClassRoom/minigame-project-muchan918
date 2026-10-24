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

    private MapData mapData;
    private Transform tileParent;

    public MapData MapData => mapData;
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
        mapData = new MapData(mapWidth, mapHeight, CurrentSeed, nearZoneRadius, midZoneRadius);

        tileParent = new GameObject("Ground").transform;
        tileParent.SetParent(transform);

        TileChunkManager.Instance.Initialize(mapData, tileParent, CurrentSeed);

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
        if (tileParent != null)
            Destroy(tileParent.gameObject);
        TileChunkManager.Instance.Clear();
    }
}
