using UnityEngine;
using UnityEngine.InputSystem; // 추가

public class DayNightCycleTest : MonoBehaviour
{
    public GameObject gameManager;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            gameManager.GetComponent<DayNightCycle>().SetMorning();
            Debug.Log("SetMorning called");
        }
    }
}
