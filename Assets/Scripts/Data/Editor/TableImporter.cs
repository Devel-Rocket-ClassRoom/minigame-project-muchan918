using System.IO;
using UnityEditor;
using UnityEngine;

public class TableImporter : EditorWindow
{
    private enum TableType
    {
        Resource,
        Item,
        Animal,
        Equipment,
        // Food,
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
            case TableType.Item:
                ImportItem();
                break;
            case TableType.Animal:
                ImportAnimal();
                break;
            case TableType.Equipment:
                ImportEquipment();
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

    private void ImportItem()
    {
        string csvPath = "Assets/Resources/DataTables/ItemTable.csv";
        string csvText = File.ReadAllText(csvPath);
        var list = DataTable.LoadCSV<ItemData>(csvText);

        string soFolder = "Assets/ScriptableObjects/Items";

        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");

        if (!AssetDatabase.IsValidFolder(soFolder))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Items");

        foreach (var data in list)
        {
            string assetPath = $"{soFolder}/{data.ItemID}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<ItemAsset>(assetPath);

            if (existing != null)
            {
                existing.ItemID = data.ItemID;
                EditorUtility.SetDirty(existing);
            }
            else
            {
                var so = CreateInstance<ItemAsset>();
                so.ItemID = data.ItemID;
                AssetDatabase.CreateAsset(so, assetPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"ItemTable Import 완료: {list.Count}개");
    }

    private void ImportAnimal()
    {
        string csvPath = "Assets/Resources/DataTables/AnimalTable.csv";
        string csvText = File.ReadAllText(csvPath);
        var list = DataTable.LoadCSV<AnimalData>(csvText);

        string passiveFolder = "Assets/ScriptableObjects/Animals/Passive";
        string hostileFolder = "Assets/ScriptableObjects/Animals/Hostile";

        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Animals"))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Animals");
        if (!AssetDatabase.IsValidFolder(passiveFolder))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects/Animals", "Passive");
        if (!AssetDatabase.IsValidFolder(hostileFolder))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects/Animals", "Hostile");

        foreach (var data in list)
        {
            if (data.AnimalType == AnimalType.Passive)
            {
                string path = $"{passiveFolder}/{data.AnimalID}.asset";
                var existing = AssetDatabase.LoadAssetAtPath<PassiveAnimalAsset>(path);
                if (existing != null)
                {
                    existing.AnimalID = data.AnimalID;
                    EditorUtility.SetDirty(existing);
                }
                else
                {
                    var so = CreateInstance<PassiveAnimalAsset>();
                    so.AnimalID = data.AnimalID;
                    AssetDatabase.CreateAsset(so, path);
                }
            }
            else
            {
                string path = $"{hostileFolder}/{data.AnimalID}.asset";
                var existing = AssetDatabase.LoadAssetAtPath<HostileAnimalAsset>(path);
                if (existing != null)
                {
                    existing.AnimalID = data.AnimalID;
                    EditorUtility.SetDirty(existing);
                }
                else
                {
                    var so = CreateInstance<HostileAnimalAsset>();
                    so.AnimalID = data.AnimalID;
                    AssetDatabase.CreateAsset(so, path);
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"AnimalTable Import 완료: {list.Count}개");
    }

    private void ImportEquipment()
    {
        string csvPath = "Assets/Resources/DataTables/EquipmentTable.csv";
        string csvText = File.ReadAllText(csvPath);
        var list = DataTable.LoadCSV<EquipmentData>(csvText);

        string soFolder = "Assets/ScriptableObjects/Equipments";

        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
        if (!AssetDatabase.IsValidFolder(soFolder))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Equipments");

        foreach (var data in list)
        {
            string assetPath = $"{soFolder}/{data.EquipmentID}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<EquipmentAsset>(assetPath);

            if (existing != null)
            {
                existing.EquipmentID = data.EquipmentID;
                EditorUtility.SetDirty(existing);
            }
            else
            {
                var so = CreateInstance<EquipmentAsset>();
                so.EquipmentID = data.EquipmentID;
                AssetDatabase.CreateAsset(so, assetPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"EquipmentTable Import 완료: {list.Count}개");
    }
}
