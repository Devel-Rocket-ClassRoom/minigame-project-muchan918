using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour, IAttacker
{
    public static PlayerAction Instance { get; private set; }

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
        Instance = this;
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
        int totalDamage = Damage;

        EquipmentData weapon = PlayerEquipment.Instance.GetWeaponRightData();
        if (weapon != null && !string.IsNullOrEmpty(weapon.TargetTag))
        {
            if ((target as Component).CompareTag(weapon.TargetTag))
            {
                totalDamage += weapon.Value;
            }
        }

        Vector3 hitNormal = (
            (target as Component).transform.position - transform.position
        ).normalized;
        target.TakeDamage(totalDamage, hitNormal);
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

    public void AddDamage(int amount)
    {
        damage = Mathf.Max(0, damage + amount);
    }
}
