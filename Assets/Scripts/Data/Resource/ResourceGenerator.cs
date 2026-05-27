using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [System.Serializable]
    public class ResourceSpawnEntry
    {
        public GameObject prefab;

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

    private IEnumerator SpawnCoroutine()
    {
        MapData mapData = tileMapGenerator.MapData;
        System.Random random = new System.Random(Random.Range(1, 999999));

        int count = 0;
        int spawnPerFrame = 300;

        foreach (var coord in mapData.GroundTiles)
        {
            float dist = Mathf.Sqrt(coord.x * coord.x + coord.y * coord.y);
            List<ResourceSpawnEntry> zone = GetZone(dist);

            foreach (var entry in zone)
            {
                if (entry.prefab == null)
                    continue;
                if (random.NextDouble() > entry.spawnChance)
                    continue;

                Instantiate(
                    entry.prefab,
                    new Vector3(coord.x, 1f, coord.y),
                    Quaternion.identity,
                    resourceParent
                );
                break;
            }

            count++;
            if (count >= spawnPerFrame)
            {
                count = 0;
                yield return null;
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
