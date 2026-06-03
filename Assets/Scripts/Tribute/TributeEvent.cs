using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TributeEvent : MonoBehaviour
{
    [System.Serializable]
    public class TributeLevelData
    {
        public List<TributeRequirement> requirements;
    }

    public List<TributeLevelData> levelPool; // 인스펙터에서 레벨별로 등록
    public UiTributeSlotList tributeSlotList;

    public int TributeLevel { get; private set; } = 0;
    public int CurrentEventLevel { get; private set; } = 0;
    public int CurrentRequirementIndex { get; private set; } = 0;
    public bool IsTributeFulfilled { get; private set; } = false;

    public string CurrentRequirementID =>
        levelPool[CurrentEventLevel].requirements[CurrentRequirementIndex].requirementID;

    public bool Evaluate()
    {
        if (tributeSlotList.IsAllComplete())
        {
            IsTributeFulfilled = true;
            Debug.Log("상납 성공");
            return true;
        }
        else
        {
            Debug.Log("상납 실패");
            return false;
        }
    }

    public void AssignNewEvent()
    {
        int sum = 0;
        if (UpgradeManager.Instance != null)
        {
            sum =
                UpgradeManager.Instance.GetLevel(UpgradeType.Workbench)
                + UpgradeManager.Instance.GetLevel(UpgradeType.Animal)
                + UpgradeManager.Instance.GetLevel(UpgradeType.Resource)
                + UpgradeManager.Instance.GetLevel(UpgradeType.Cauldron);
        }

        TributeLevel = sum switch
        {
            <= 3 => 0,
            <= 6 => 1,
            <= 9 => 2,
            _ => 3,
        };

        IsTributeFulfilled = false;
        CurrentEventLevel = TributeLevel;
        var pool = levelPool[TributeLevel].requirements;
        CurrentRequirementIndex = Random.Range(0, pool.Count);
        tributeSlotList.Setup(pool[CurrentRequirementIndex]);
    }

    public void SetRequirement(int tributeLevel, int eventLevel, int index)
    {
        TributeLevel = tributeLevel;
        CurrentEventLevel = eventLevel;
        CurrentRequirementIndex = index;
        IsTributeFulfilled = false;
        tributeSlotList.Setup(levelPool[eventLevel].requirements[index]);
    }

    public void SetRequirementByID(int tributeLevel, int eventLevel, string requirementID)
    {
        TributeLevel = tributeLevel;
        CurrentEventLevel = eventLevel;
        IsTributeFulfilled = false;

        var pool = levelPool[eventLevel].requirements;
        int index = pool.FindIndex(r => r.requirementID == requirementID);
        if (index == -1)
        {
            Debug.LogWarning(
                $"[TributeEvent] requirementID 못 찾음: {requirementID}, 0번으로 대체"
            );
            index = 0;
        }
        CurrentRequirementIndex = index;
        tributeSlotList.Setup(pool[CurrentRequirementIndex]);
    }
}
