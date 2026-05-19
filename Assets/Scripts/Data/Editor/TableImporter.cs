using System.IO;
using UnityEditor;
using UnityEngine;

public class TableImporter : EditorWindow
{
    private enum TableType
    {
        Resource,
        DropItem,
        // Animal,
        // Equip,
    }

    private TableType selectedTable = TableType.Resource;

    [MenuItem("Tools/Table Importer")]
    public static void Open()
    {
        GetWindow<TableImporter>("Table Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Table Importer", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        selectedTable = (TableType)EditorGUILayout.EnumPopup("Table Type", selectedTable);
        EditorGUILayout.Space();

        if (GUILayout.Button("Import"))
        {
            Import(selectedTable);
        }
    }

    private void Import(TableType type)
    {
        switch (type)
        {
            case TableType.Resource:
                ImportResource();
                break;
            case TableType.DropItem:
                ImportDropItem();
                break;
        }
    }

    private void ImportResource()
    {
        string csvPath = "Assets/Resources/DataTables/ResourceTable.csv";
        string csvText = File.ReadAllText(csvPath);
        var list = DataTable.LoadCSV<ResourceData>(csvText);

        string soFolder = "Assets/ScriptableObjects/Resources";

        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");

        if (!AssetDatabase.IsValidFolder(soFolder))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Resources");

        foreach (var data in list)
        {
            string assetPath = $"{soFolder}/{data.ResourceID}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<ResourceAsset>(assetPath);

            if (existing != null)
            {
                // 이미 있으면 ID만 갱신 (Prefab, Icon 연결은 유지)
                existing.ResourceID = data.ResourceID;
                EditorUtility.SetDirty(existing);
            }
            else
            {
                // 없으면 새로 생성
                var so = CreateInstance<ResourceAsset>();
                so.ResourceID = data.ResourceID;
                AssetDatabase.CreateAsset(so, assetPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"ResourceTable Import 완료: {list.Count}개");
    }

    private void ImportDropItem()
    {
        string csvPath = "Assets/Resources/DataTables/DropItemTable.csv";
        string csvText = File.ReadAllText(csvPath);
        var list = DataTable.LoadCSV<DropItemData>(csvText);

        string soFolder = "Assets/ScriptableObjects/DropItems";

        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");

        if (!AssetDatabase.IsValidFolder(soFolder))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "DropItems");

        foreach (var data in list)
        {
            string assetPath = $"{soFolder}/{data.DropItemID}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<DropItemAsset>(assetPath);

            if (existing != null)
            {
                existing.DropItemID = data.DropItemID;
                EditorUtility.SetDirty(existing);
            }
            else
            {
                var so = CreateInstance<DropItemAsset>();
                so.DropItemID = data.DropItemID;
                AssetDatabase.CreateAsset(so, assetPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"DropItemTable Import 완료: {list.Count}개");
    }
}
