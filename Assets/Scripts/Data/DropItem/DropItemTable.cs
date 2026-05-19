using System.Collections.Generic;
using UnityEngine;

public class DropItemTable : DataTable
{
    public readonly Dictionary<string, DropItemData> table = new Dictionary<string, DropItemData>();

    public override void Load(string filename)
    {
        table.Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        List<DropItemData> list = LoadCSV<DropItemData>(textAsset.text);

        foreach (var data in list)
        {
            if (!table.ContainsKey(data.DropItemID))
                table.Add(data.DropItemID, data);
            else
                Debug.LogError($"DropItemID 중복: {data.DropItemID}");
        }
    }

    public DropItemData Get(string id)
    {
        if (!table.TryGetValue(id, out var data))
        {
            Debug.LogError($"DropItemID 없음: {id}");
            return null;
        }
        return data;
    }
}
