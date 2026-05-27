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
        Agent.SetDestination(PlayerTransform.position);

        if (Agent.velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(
                new Vector3(Agent.velocity.x, 0f, Agent.velocity.z)
            );
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                10f * Time.deltaTime
            );
        }

        if (
            Vector3.Distance(transform.position, PlayerTransform.position)
            <= HostileAsset.EnterAttackRange
        )
        {
            Agent.ResetPath();
            CurrentState = AnimalState.Attack;
        }
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
                Animator.SetInteger("State", 0);
                break;
            case AnimalState.Roam:
                Animator.SetInteger("State", 1);
                break;
            case AnimalState.Chase:
                Animator.SetInteger("State", 2);
                break;
            case AnimalState.Attack:
                Animator.SetInteger("State", 3);
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
