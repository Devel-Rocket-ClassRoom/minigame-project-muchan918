using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [System.Serializable]
    public class ResourceSpawnEntry
    {
        public GameObject prefab;
        public TileType spawnTileType;

        [Range(0f, 1f)]
        public float spawnChance;
    }

    [Header("Zone 경계 (중심에서 거리 기준)")]
    [SerializeField]
    private float nearZoneRadius = 50f;

    [SerializeField]
    private float midZoneRadius = 100f;

    [Header("Near Zone (희귀한 것 먼저)")]
    [SerializeField]
    private List<ResourceSpawnEntry> nearZone;

    [Header("Mid Zone (희귀한 것 먼저)")]
    [SerializeField]
    private List<ResourceSpawnEntry> midZone;

    [Header("Far Zone (희귀한 것 먼저)")]
    [SerializeField]
    private List<ResourceSpawnEntry> farZone;

    private TileMapGenerator tileMapGenerator;

    private Transform resourceParent;

    private void Awake()
    {
        tileMapGenerator = GetComponent<TileMapGenerator>();
    }

    public void Generate()
    {
        if (resourceParent != null)
            Destroy(resourceParent.gameObject);

        resourceParent = new GameObject("Resources").transform;
        resourceParent.SetParent(transform);

        StartCoroutine(SpawnCoroutine());
    }

    public void Respawn()
    {
        Generate();
    }

    private IEnumerator SpawnCoroutine()
    {
        MapData mapData = tileMapGenerator.MapData;
        System.Random random = new System.Random(Random.Range(1, 999999));

        int halfWidth = mapData.Width / 2;
        int halfHeight = mapData.Height / 2;
        int baseHalf = 15;

        int count = 0;
        int spawnPerFrame = 100;

        for (int y = 0; y < mapData.Height; y++)
        {
            for (int x = 0; x < mapData.Width; x++)
            {
                int wx = x - halfWidth;
                int wy = y - halfHeight;

                if (wx >= -baseHalf && wx < baseHalf && wy >= -baseHalf && wy < baseHalf)
                    continue;

                TileType tileType = mapData.GetTile(x, y);
                float dist = Mathf.Sqrt(wx * wx + wy * wy);
                List<ResourceSpawnEntry> zone = GetZone(dist);
                Vector3 pos = new Vector3(wx, 1f, wy);

                foreach (var entry in zone)
                {
                    if (entry.prefab == null)
                        continue;
                    if (entry.spawnTileType != tileType)
                        continue;
                    if (random.NextDouble() > entry.spawnChance)
                        continue;

                    Instantiate(entry.prefab, pos, Quaternion.identity, resourceParent);
                    break;
                }

                count++;
                if (count >= spawnPerFrame)
                {
                    count = 0;
                    yield return null;
                }
            }
        }

        Debug.Log("자원 생성 완료");
        GamePause.Resume();
    }

    private List<ResourceSpawnEntry> GetZone(float distance)
    {
        if (distance <= nearZoneRadius)
            return nearZone;
        if (distance <= midZoneRadius)
            return midZone;
        return farZone;
    }
}
