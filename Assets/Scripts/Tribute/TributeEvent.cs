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

    private void Update()
    {
        // 임시 레벨업 테스트 코드
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            LevelUp();
            Debug.Log($"[TributeEvent] 레벨업 → Level {TributeLevel}");
        }
    }

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
        IsTributeFulfilled = false;
        CurrentEventLevel = TributeLevel;
        var pool = levelPool[CurrentEventLevel].requirements;
        CurrentRequirementIndex = Random.Range(0, pool.Count);
        tributeSlotList.Setup(pool[CurrentRequirementIndex]);
    }

    public void LevelUp()
    {
        TributeLevel = Mathf.Min(TributeLevel + 1, levelPool.Count - 1);
        Debug.Log($"[TributeEvent] 레벨업 → TributeLevel {TributeLevel}");
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
