using System.Collections.Generic;
using UnityEngine;

public class TributeEvent : MonoBehaviour
{
    public List<TributeRequirement> requirementPool;
    public UiTributeSlotList tributeSlotList;

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

        int randomIndex = Random.Range(0, requirementPool.Count);
        tributeSlotList.Setup(requirementPool[randomIndex]);
    }
}
