using System.Collections.Generic;
using UnityEngine;

public class ItemTable : DataTable
{
    public readonly Dictionary<string, ItemData> table = new Dictionary<string, ItemData>();

    public override void Load(string filename)
    {
        table.Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        List<ItemData> list = LoadCSV<ItemData>(textAsset.text);

        foreach (var data in list)
        {
            if (!table.ContainsKey(data.ItemID))
                table.Add(data.ItemID, data);
            else
                Debug.LogError($"ItemID 중복: {data.ItemID}");
        }
    }

    public ItemData Get(string id)
    {
        if (!table.TryGetValue(id, out var data))
        {
            Debug.LogError($"DropItemID 없음: {id}");
            return null;
        }
        return data;
    }
}
