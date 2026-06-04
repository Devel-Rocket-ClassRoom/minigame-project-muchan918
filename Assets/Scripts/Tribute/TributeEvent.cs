using System.Collections.Generic;
using UnityEngine;

public class TributeEvent : MonoBehaviour
{
    public List<TributeRequirement> requirements;
    public UiTributeSlotList tributeSlotList;

    public int CurrentRequirementIndex { get; private set; } = 0;
    public bool IsTributeFulfilled { get; private set; } = false;

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
        if (CurrentRequirementIndex < requirements.Count - 1)
            CurrentRequirementIndex++;
        tributeSlotList.Setup(requirements[CurrentRequirementIndex]);
    }

    public void SetRequirementIndex(int index)
    {
        CurrentRequirementIndex = index;
        IsTributeFulfilled = false;
        tributeSlotList.Setup(requirements[CurrentRequirementIndex]);
    }
}
