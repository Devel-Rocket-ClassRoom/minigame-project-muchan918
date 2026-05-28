using System.Collections.Generic;
using UnityEngine;

public class EquipmentTable : DataTable
{
    public readonly Dictionary<string, EquipmentData> table =
        new Dictionary<string, EquipmentData>();

    public override void Load(string filename)
    {
        table.Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<EquipmentData>(textAsset.text);

        foreach (var data in list)
        {
            if (!table.ContainsKey(data.EquipmentID))
                table.Add(data.EquipmentID, data);
            else
                Debug.LogError($"EquipmentID 중복: {data.EquipmentID}");
        }
    }

    public EquipmentData Get(string id)
    {
        if (!table.TryGetValue(id, out var data))
        {
            Debug.LogError($"EquipmentID 없음: {id}");
            return null;
        }
        return data;
    }
}
