using UnityEngine;

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

    private AnimalState currentState;
    protected AnimalState PrevState { get; private set; }
    protected AnimalState CurrentState
    {
        get => currentState;
        set
        {
            PrevState = currentState;
            currentState = value;
            OnStateChanged(value);
        }
    }

    private float stateTimer;
    protected Vector3 MoveDirection { get; set; }

    protected virtual void Start()
    {
        Asset.Data = DataTableManager.Get<AnimalTable>("AnimalTable").Get(Asset.AnimalID);
        currentHp = Asset.Data.MaxHP;
        PlayerTransform = PlayerSpawner.Instance.PlayerTransform;
        Animator = GetComponent<Animator>();
        SetAnimatorController();

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
    }

    private void UpdateIdle()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            MoveDirection = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            ).normalized;
            transform.forward = MoveDirection;
            CurrentState = AnimalState.Roam;
            stateTimer = Random.Range(Asset.RoamDurationMin, Asset.RoamDurationMax);
        }
    }

    private void UpdateRoam()
    {
        transform.position += MoveDirection * Asset.Data.MoveSpeed * Time.deltaTime;

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            CurrentState = AnimalState.Idle;
            stateTimer = Random.Range(Asset.IdleDurationMin, Asset.IdleDurationMax);
        }
    }

    protected virtual void OnStateChanged(AnimalState newState) { }

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
