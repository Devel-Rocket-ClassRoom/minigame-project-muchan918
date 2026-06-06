using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(ResourceGenerator.ResourceSpawnEntry))]
public class ResourceSpawnEntryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineH = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        Rect rect = new Rect(position.x, position.y, position.width, lineH);

        EditorGUI.PropertyField(rect, property.FindPropertyRelative("prefab"));
        rect.y += lineH + spacing;

        var useSpread = property.FindPropertyRelative("useSpread");
        EditorGUI.PropertyField(rect, useSpread);
        rect.y += lineH + spacing;

        if (useSpread.boolValue)
        {
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("spreadSeedCount"));
            rect.y += lineH + spacing;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("spreadDepth"));
            rect.y += lineH + spacing;
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("spreadChance"));
        }
        else
        {
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("weight"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineH = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        bool useSpread = property.FindPropertyRelative("useSpread").boolValue;
        int lines = useSpread ? 5 : 3;
        return lines * (lineH + spacing);
    }
}
#endif

public class ResourceGenerator : MonoBehaviour, IUpgradeable
{
    [System.Serializable]
    public class ResourceSpawnEntry
    {
        public GameObject prefab; // null이면 빈칸

        [Min(1)]
        public int weight = 10;

        public bool useSpread;
        public int spreadSeedCount = 3;
        public int spreadDepth = 5;

        [Range(0f, 1f)]
        public float spreadChance = 0.55f;
    }

    [System.Serializable]
    public class ResourceUpgradeLevel
    {
        public List<ResourceSpawnEntry> nearZone;
        public List<ResourceSpawnEntry> midZone;
        public List<ResourceSpawnEntry> farZone;
    }

    [SerializeField]
    private List<ResourceUpgradeLevel> spawnEntriesByLevel;

    private List<ResourceSpawnEntry> nearZone;
    private List<ResourceSpawnEntry> midZone;
    private List<ResourceSpawnEntry> farZone;

    private TileMapGenerator tileMapGenerator;
    private Transform resourceParent;

    public int Level { get; private set; }

    private void Awake()
    {
        tileMapGenerator = GetComponent<TileMapGenerator>();

        if (spawnEntriesByLevel.Count > 0)
            ApplyLevel(0);
    }

    public void Upgrade()
    {
        if (Level >= spawnEntriesByLevel.Count - 1)
            return;

        Level++;
        ApplyLevel(Level);
    }

    private void ApplyLevel(int level)
    {
        var l = spawnEntriesByLevel[level];
        nearZone = l.nearZone;
        midZone = l.midZone;
        farZone = l.farZone;
    }

    public void Generate()
    {
        if (resourceParent != null)
            Destroy(resourceParent.gameObject);

        resourceParent = new GameObject("Resources").transform;
        resourceParent.SetParent(transform);

        ResourceChunkManager.Instance.Initialize(resourceParent);
    }

    public IEnumerator SpawnCoroutine()
    {
        Generate();

        MapData mapData = tileMapGenerator.MapData;
        System.Random random = new System.Random(tileMapGenerator.CurrentSeed);

        yield return RegisterZone(mapData.NearTiles, nearZone, mapData, random);
        yield return RegisterZone(mapData.MidTiles, midZone, mapData, random);
        yield return RegisterZone(mapData.FarTiles, farZone, mapData, random);
    }

    private IEnumerator RegisterZone(
        List<Vector2Int> tiles,
        List<ResourceSpawnEntry> zone,
        MapData mapData,
        System.Random random
    )
    {
        int count = 0;
        const int perFrame = 500;

        // Spread 먼저
        foreach (var entry in zone)
        {
            if (!entry.useSpread)
                continue;

            for (int i = 0; i < entry.spreadSeedCount; i++)
            {
                if (tiles.Count == 0)
                    break;
                Vector2Int seed = tiles[random.Next(0, tiles.Count)];
                SpreadResource(seed, entry, mapData, random);
            }

            count++;
            if (count >= perFrame)
            {
                count = 0;
                yield return null;
            }
        }

        // 가중치 합산
        int totalWeight = 0;
        foreach (var entry in zone)
            if (!entry.useSpread)
                totalWeight += entry.weight;

        if (totalWeight == 0)
            yield break;

        // 타일마다 가중치 룰렛
        foreach (var coord in tiles)
        {
            var tileType = mapData.GetTileWorld(coord);
            if (tileType == TileType.Ground || tileType == TileType.GrassGround)
            {
                int roll = random.Next(0, totalWeight);
                int cursor = 0;

                foreach (var entry in zone)
                {
                    if (entry.useSpread)
                        continue;

                    cursor += entry.weight;
                    if (roll < cursor)
                    {
                        if (entry.prefab != null)
                        {
                            ResourceChunkManager.Instance.RegisterSpawnInfo(
                                new Vector3(coord.x, 1f, coord.y),
                                entry.prefab
                            );
                            mapData.SetTile(coord, TileType.Resource);
                        }
                        break;
                    }
                }
            }

            count++;
            if (count >= perFrame)
            {
                count = 0;
                yield return null;
            }
        }
    }

    private void SpreadResource(
        Vector2Int origin,
        ResourceSpawnEntry entry,
        MapData mapData,
        System.Random random
    )
    {
        SpreadFrom(origin, entry, entry.spreadDepth, mapData, random);
    }

    private void SpreadFrom(
        Vector2Int coord,
        ResourceSpawnEntry entry,
        int depth,
        MapData mapData,
        System.Random random
    )
    {
        if (depth <= 0)
            return;

        var tileType = mapData.GetTileWorld(coord);
        if (tileType != TileType.Ground && tileType != TileType.GrassGround)
            return;

        ResourceChunkManager.Instance.RegisterSpawnInfo(
            new Vector3(coord.x, 1f, coord.y),
            entry.prefab
        );
        mapData.SetTile(coord, TileType.Resource);

        if (random.NextDouble() < entry.spreadChance)
            SpreadFrom(coord + Vector2Int.right, entry, depth - 1, mapData, random);
        if (random.NextDouble() < entry.spreadChance)
            SpreadFrom(coord + Vector2Int.left, entry, depth - 1, mapData, random);
        if (random.NextDouble() < entry.spreadChance)
            SpreadFrom(coord + Vector2Int.up, entry, depth - 1, mapData, random);
        if (random.NextDouble() < entry.spreadChance)
            SpreadFrom(coord + Vector2Int.down, entry, depth - 1, mapData, random);
    }
}
