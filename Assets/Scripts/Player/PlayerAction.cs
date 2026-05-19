using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    private Animator animator;
    private static readonly int ActionHash = Animator.StringToHash("Action");

    public bool IsActing { get; private set; }

    [SerializeField]
    private float actionCooldown = 0.6f;

    private Coroutine actionCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.isPressed && !IsActing)
            PerformAction();
    }

    private void PerformAction()
    {
        animator.SetTrigger(ActionHash);

        if (actionCoroutine != null)
            StopCoroutine(actionCoroutine);

        actionCoroutine = StartCoroutine(ActionCooldown());
    }

    private IEnumerator ActionCooldown()
    {
        IsActing = true;
        yield return new WaitForSeconds(actionCooldown);
        IsActing = false;
    }
}
