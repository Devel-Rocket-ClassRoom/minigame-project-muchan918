using System.Collections.Generic;
using UnityEngine;

public class AnimalTable : DataTable
{
    public readonly Dictionary<string, AnimalData> table = new Dictionary<string, AnimalData>();

    public override void Load(string filename)
    {
        table.Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        List<AnimalData> list = LoadCSV<AnimalData>(textAsset.text);

        foreach (var data in list)
        {
            if (!table.ContainsKey(data.AnimalID))
                table.Add(data.AnimalID, data);
            else
                Debug.LogError($"AnimalID 중복: {data.AnimalID}");
        }
    }

    public AnimalData Get(string id)
    {
        if (!table.TryGetValue(id, out var data))
        {
            Debug.LogError($"AnimalID 없음: {id}");
            return null;
        }
        return data;
    }
}
