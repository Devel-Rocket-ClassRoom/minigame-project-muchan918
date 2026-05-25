using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables =
        new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        Init();
    }

    // 여기에 사용할 테이블 초기화
    private static void Init()
    {
        // Resource
        var resourceTable = new ResourceTable();
        resourceTable.Load("ResourceTable");
        tables.Add("ResourceTable", resourceTable);

        // Item
        var itemTable = new ItemTable();
        itemTable.Load("ItemTable");
        tables.Add("ItemTable", itemTable);

        // Animal
        var animalTable = new AnimalTable();
        animalTable.Load("AnimalTable");
        tables.Add("AnimalTable", animalTable);
    }

    public static T Get<T>(string id)
        where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return null;
        }

        return tables[id] as T;
    }
}
