using UnityEngine;

public class PassiveAnimal : Animal
{
    private PassiveAnimalAsset PassiveAsset => Asset as PassiveAnimalAsset;

    private float fleeTimer;

    protected override void Update()
    {
        base.Update();

        if (CurrentState == AnimalState.Flee)
            UpdateFlee();
    }

    private void UpdateFlee()
    {
        Vector3 dirFromPlayer = (transform.position - PlayerTransform.position).normalized;
        MoveDirection = new Vector3(dirFromPlayer.x, 0f, dirFromPlayer.z).normalized;
        transform.forward = MoveDirection;
        transform.position += MoveDirection * Asset.Data.MoveSpeed * Time.deltaTime;

        fleeTimer -= Time.deltaTime;
        if (fleeTimer <= 0f)
        {
            ResetStateTimer();
            CurrentState = AnimalState.Idle;
        }
    }

    protected override void OnTakeDamage(Vector3 hitNormal)
    {
        fleeTimer = PassiveAsset.FleeDuration;
        CurrentState = AnimalState.Flee;
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
            case AnimalState.Flee:
                Animator.SetInteger("State", 2);
                break;
        }
    }

    protected override void SetAnimatorController()
    {
        Animator.runtimeAnimatorController = PassiveAsset.AnimatorController;
    }
}
