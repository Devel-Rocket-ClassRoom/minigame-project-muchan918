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
        transform.position += MoveDirection * Asset.Data.MoveSpeed * Time.deltaTime;

        fleeTimer -= Time.deltaTime;
        if (fleeTimer <= 0f)
            CurrentState = AnimalState.Idle;
    }

    protected override void OnTakeDamage(Vector3 hitNormal)
    {
        Vector3 dirFromPlayer = (transform.position - PlayerTransform.position).normalized;
        MoveDirection = new Vector3(dirFromPlayer.x, 0f, dirFromPlayer.z).normalized;
        transform.forward = MoveDirection;

        fleeTimer = PassiveAsset.FleeDuration;
        CurrentState = AnimalState.Flee;
    }
}
