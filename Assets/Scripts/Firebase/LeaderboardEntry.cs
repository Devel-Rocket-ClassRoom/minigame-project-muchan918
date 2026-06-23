using System;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string userId;
    public string nickname;
    public int survivedDays;
    public int killedAnimals;
    public long timestamp;

    public LeaderboardEntry() { }

    public LeaderboardEntry(string userId, string nickname, int survivedDays, int killedAnimals)
    {
        this.userId = userId;
        this.nickname = nickname;
        this.survivedDays = survivedDays;
        this.killedAnimals = killedAnimals;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static LeaderboardEntry FromJson(string json)
    {
        return JsonUtility.FromJson<LeaderboardEntry>(json);
    }
}