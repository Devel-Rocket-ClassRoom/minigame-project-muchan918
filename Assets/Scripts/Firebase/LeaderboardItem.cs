using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nicknameText;
    [SerializeField] private TextMeshProUGUI valueText;

    public void SetData(int rank, LeaderboardEntry entry, LeaderboardType type)
    {
        rankText.text = $"{rank}";
        nicknameText.text = entry.nickname;

        if (type == LeaderboardType.SurvivedDays)
            valueText.text = $"{entry.survivedDays}일";
        else if (type == LeaderboardType.KilledAnimals)
            valueText.text = $"{entry.killedAnimals}마리";
    }
}