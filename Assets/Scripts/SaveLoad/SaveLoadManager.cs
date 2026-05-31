using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using SaveDataVC = SaveDataV5;

public static class SaveLoadManager
{
    public enum SaveMode
    {
        Text, // .json
        Encrypted, // .dat
    }

    public static SaveMode Mode { get; set; } = SaveMode.Text; // 여기서 모드 전환

    private static readonly int SaveDataVersion = 5;
    private static readonly string SaveDirectory = Path.Combine(
        Application.persistentDataPath,
        "Save"
    );

    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
    };

    public static SaveDataVC Data { get; set; } = new SaveDataVC();

    private static string SaveFilePath =>
        Mode == SaveMode.Text
            ? Path.Combine(SaveDirectory, "SaveAuto.json")
            : Path.Combine(SaveDirectory, "SaveAuto.dat");

    public static void Init()
    {
        Load();
    }

    public static bool Save()
    {
        if (Data == null)
            return false;

        try
        {
            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);

            string json = JsonConvert.SerializeObject(Data, Settings);

            if (Mode == SaveMode.Text)
                File.WriteAllText(SaveFilePath, json);
            else
                File.WriteAllBytes(SaveFilePath, CryptoUtil.Encrypt(json));

            return true;
        }
        catch
        {
            Debug.LogError("[SaveLoadManager] Save 실패");
            return false;
        }
    }

    public static bool Load()
    {
        if (!File.Exists(SaveFilePath))
            return false;

        try
        {
            string json;

            if (Mode == SaveMode.Text)
                json = File.ReadAllText(SaveFilePath);
            else
                json = CryptoUtil.Decrypt(File.ReadAllBytes(SaveFilePath));

            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json, Settings);

            while (saveData.Version < SaveDataVersion)
                saveData = saveData.VersionUp();

            Data = saveData as SaveDataVC;
            return true;
        }
        catch
        {
            Debug.LogError("[SaveLoadManager] Load 실패");
            return false;
        }
    }

    public static bool HasSaveData()
    {
        return File.Exists(SaveFilePath);
    }

    public static void DeleteSave()
    {
        if (File.Exists(SaveFilePath))
            File.Delete(SaveFilePath);
        Data = new SaveDataVC();
    }
}
