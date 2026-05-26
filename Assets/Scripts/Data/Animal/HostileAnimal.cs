using UnityEngine;

public class HostileAnimal : Animal, IAttacker
{
    private HostileAnimalAsset HostileAsset => Asset as HostileAnimalAsset;

    public int Damage => HostileAsset.Damage;

    private bool isAttacking;

    protected override void Start()
    {
        base.Start();
        PlayerHealth.OnPlayerDied += OnPlayerDied;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDied -= OnPlayerDied;
    }

    protected override void Update()
    {
        base.Update();

        switch (CurrentState)
        {
            case AnimalState.Chase:
                UpdateChase();
                break;
            case AnimalState.Attack:
                UpdateAttack();
                break;
        }
    }

    private void UpdateChase()
    {
        Vector3 dir = (PlayerTransform.position - transform.position).normalized;
        dir.y = 0f;
        transform.forward = dir;
        transform.position += dir * Asset.Data.MoveSpeed * Time.deltaTime;

        if (
            Vector3.Distance(transform.position, PlayerTransform.position)
            <= HostileAsset.EnterAttackRange
        )
            CurrentState = AnimalState.Attack;
    }

    private void UpdateAttack()
    {
        Vector3 lookAt = PlayerTransform.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if (
            !isAttacking
            && Vector3.Distance(transform.position, PlayerTransform.position)
                > HostileAsset.ExitAttackRange
        )
        {
            CurrentState = AnimalState.Chase;
            return;
        }
    }

    public void OnAttackStart() => isAttacking = true;

    // 애니메이션 이벤트에서 호출
    public void OnAttackHit()
    {
        if (
            Vector3.Distance(transform.position, PlayerTransform.position)
            <= HostileAsset.ExitAttackRange
        )
            Attack(PlayerTransform.GetComponent<IDefender>());

        isAttacking = false;
    }

    public void Attack(IDefender target)
    {
        if (target == null)
            return;

        Vector3 hitNormal = (
            (target as Component).transform.position - transform.position
        ).normalized;
        target.TakeDamage(Damage, hitNormal);
    }

    protected override void OnTakeDamage(Vector3 hitNormal)
    {
        if (CurrentState == AnimalState.Chase || CurrentState == AnimalState.Attack)
            return;

        CurrentState = AnimalState.Chase;
    }

    public void OnPlayerDied()
    {
        isAttacking = false;
        CurrentState = AnimalState.Idle;
    }

    protected override void OnStateChanged(AnimalState newState)
    {
        switch (newState)
        {
            case AnimalState.Idle:
                Animator.SetTrigger(
                    PrevState == AnimalState.Chase || PrevState == AnimalState.Attack
                        ? "Stop"
                        : "Idle"
                );
                break;
            case AnimalState.Roam:
                Animator.SetTrigger("Walk");
                break;
            case AnimalState.Chase:
                Animator.SetTrigger("Run");
                break;
            case AnimalState.Attack:
                Animator.SetTrigger("Attack");
                break;
        }
    }

    protected override void SetAnimatorController()
    {
        Animator.runtimeAnimatorController = HostileAsset.AnimatorController;
    }

    private void OnDrawGizmos()
    {
        if (HostileAsset == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, HostileAsset.EnterAttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, HostileAsset.ExitAttackRange);
    }
}
