using UnityEngine;
using UnityEngine.InputSystem;

public class TributeEventTest : MonoBehaviour
{
    public TributeEvent tributeEvent;

    private void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
            tributeEvent.AssignNewEvent();
    }
}
