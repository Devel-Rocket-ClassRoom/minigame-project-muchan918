using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour, IAttacker
{
    private Animator animator;
    private static readonly int ActionHash = Animator.StringToHash("Action");

    public bool IsActing { get; private set; }

    [SerializeField]
    private float actionCooldown = 0.6f;

    [SerializeField]
    private int damage = 10;

    [SerializeField]
    private Collider hitbox;

    [SerializeField]
    private LayerMask targetLayers;

    public int Damage => damage;

    private Coroutine actionCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !IsActing)
            PerformAction();
    }

    public void PerformAction()
    {
        if (IsActing)
            return;

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

    public void Attack(IDefender target)
    {
        Vector3 hitNormal = (
            (target as Component).transform.position - transform.position
        ).normalized;
        target.TakeDamage(Damage, hitNormal);
    }

    public void OnActionHit()
    {
        if (hitbox == null)
            return;

        Collider[] hits = Physics.OverlapBox(
            hitbox.bounds.center,
            hitbox.bounds.extents,
            hitbox.transform.rotation,
            targetLayers
        );

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDefender>(out var defender))
                Attack(defender);
        }
    }
}
