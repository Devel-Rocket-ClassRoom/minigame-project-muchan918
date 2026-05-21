using UnityEngine;

public class TributeEvent : MonoBehaviour
{
    public bool IsTributeFulfilled { get; set; } = false;

    public bool Evaluate()
    {
        if (IsTributeFulfilled)
        {
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
        IsTributeFulfilled = true;
        // 나중에 요구사항 랜덤 생성 추가
    }
}
