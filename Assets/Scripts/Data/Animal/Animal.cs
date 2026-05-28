using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    Idle,
    Roam,
    Flee,
    Chase,
    Attack,
}

public abstract class Animal : MonoBehaviour, IDefender, IDroppable
{
    public AnimalAsset Asset;

    private int currentHp;
    public int MaxHp => Asset.Data.MaxHP;
    public int CurrentHp => currentHp;

    protected Transform PlayerTransform { get; private set; }
    protected Animator Animator { get; private set; }
    protected NavMeshAgent Agent { get; private set; }

    private AnimalState currentState;
    protected AnimalState PrevState { get; private set; }
    protected AnimalState CurrentState
    {
        get => currentState;
        set
        {
            PrevState = currentState;
            currentState = value;
            // Debug.Log($"[{gameObject.name}] State: {PrevState} → {currentState}");
            OnStateChanged(value);
        }
    }

    private float stateTimer;
    protected Vector3 MoveDirection { get; set; }

    // 현재 속한 청크 좌표
    private Vector2Int _currentChunk;

    protected virtual void Start()
    {
        Asset.Data = DataTableManager.Get<AnimalTable>("AnimalTable").Get(Asset.AnimalID);
        currentHp = Asset.Data.MaxHP;
        PlayerTransform = PlayerSpawner.Instance.PlayerTransform;
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = Asset.Data.MoveSpeed;
        Agent.acceleration = 100f;
        Agent.updateRotation = false;
        SetAnimatorController();

        // 초기 청크 좌표 저장
        _currentChunk = AnimalChunkManager.Instance.WorldToChunk(transform.position);

        CurrentState = AnimalState.Idle;
        stateTimer = Random.Range(Asset.IdleDurationMin, Asset.IdleDurationMax);
    }

    protected virtual void SetAnimatorController() { }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case AnimalState.Idle:
                UpdateIdle();
                break;
            case AnimalState.Roam:
                UpdateRoam();
                break;
        }

        // 청크 이동 감지
        var newChunk = AnimalChunkManager.Instance.WorldToChunk(transform.position);
        if (newChunk != _currentChunk)
        {
            AnimalChunkManager.Instance.UpdateAnimalChunk(this, _currentChunk, newChunk);
            _currentChunk = newChunk;
        }
    }

    private void UpdateIdle()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            Vector3 randomDir = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            ).normalized;
            Vector3 targetPos = transform.position + randomDir * Random.Range(3f, 8f);

            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                Agent.SetDestination(hit.position);
                CurrentState = AnimalState.Roam;
            }
            else
            {
                // 유효한 위치 못 찾으면 타이머 리셋하고 다시 대기
                stateTimer = Random.Range(Asset.IdleDurationMin, Asset.IdleDurationMax);
            }
        }
    }

    private void UpdateRoam()
    {
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

        if (Agent.pathPending)
            return;

        if (
            Agent.remainingDistance <= Agent.stoppingDistance
            && Agent.velocity.sqrMagnitude < 0.01f
        )
        {
            Agent.ResetPath();
            stateTimer = Random.Range(Asset.IdleDurationMin, Asset.IdleDurationMax);
            CurrentState = AnimalState.Idle;
        }
    }

    protected virtual void OnStateChanged(AnimalState newState) { }

    public void OnActivate()
    {
        Agent.enabled = true;
        CurrentState = AnimalState.Idle;
    }

    public void OnDeactivate()
    {
        if (Agent.enabled)
            Agent.ResetPath();
        Agent.enabled = false;
        CurrentState = AnimalState.Idle;
    }

    public void TakeDamage(int damage, Vector3 hitNormal)
    {
        currentHp -= damage;
        OnTakeDamage(hitNormal);

        if (currentHp <= 0)
        {
            Drop();
            Die();
        }
    }

    protected abstract void OnTakeDamage(Vector3 hitNormal);

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Drop()
    {
        if (Asset.DropPrefab != null)
            Instantiate(Asset.DropPrefab, transform.position, Quaternion.identity);
    }

    protected void ResetStateTimer()
    {
        stateTimer = Random.Range(Asset.IdleDurationMin, Asset.IdleDurationMax);
    }
}
