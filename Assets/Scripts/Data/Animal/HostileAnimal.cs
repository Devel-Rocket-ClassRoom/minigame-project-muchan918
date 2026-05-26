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
            <= HostileAsset.AttackRange
        )
            CurrentState = AnimalState.Attack;
    }

    private void UpdateAttack()
    {
        Vector3 lookAt = PlayerTransform.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        float currentRange = isAttacking ? HostileAsset.RealAttackRange : HostileAsset.AttackRange;
        if (Vector3.Distance(transform.position, PlayerTransform.position) > currentRange)
        {
            CurrentState = AnimalState.Chase;
            return;
        }
    }

    public void OnAttackStart() => isAttacking = true;

    public void OnAttackEnd() => isAttacking = false;

    // 애니메이션 이벤트에서 호출
    public void OnAttackHit()
    {
        if (
            Vector3.Distance(transform.position, PlayerTransform.position)
            <= HostileAsset.RealAttackRange
        )
            Attack(PlayerTransform.GetComponent<IDefender>());
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
}
