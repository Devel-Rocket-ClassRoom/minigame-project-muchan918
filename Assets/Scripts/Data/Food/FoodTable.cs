using System.Collections.Generic;
using UnityEngine;

public class FoodTable : DataTable
{
    public readonly Dictionary<string, FoodData> table = new Dictionary<string, FoodData>();

    public override void Load(string filename)
    {
        table.Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<FoodData>(textAsset.text);

        foreach (var data in list)
        {
            if (!table.ContainsKey(data.FoodID))
                table.Add(data.FoodID, data);
            else
                Debug.LogError($"FoodID 중복: {data.FoodID}");
        }
    }

    public FoodData Get(string id)
    {
        if (!table.TryGetValue(id, out var data))
        {
            Debug.LogError($"FoodID 없음: {id}");
            return null;
        }
        return data;
    }
}
