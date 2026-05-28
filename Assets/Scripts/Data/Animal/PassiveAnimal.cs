using UnityEngine;
using UnityEngine.AI;

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
        Vector3 fleeDir = (transform.position - PlayerTransform.position).normalized;
        Agent.Move(fleeDir * Asset.Data.MoveSpeed * Time.deltaTime);

        // velocity 대신 fleeDir로 회전
        if (fleeDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(fleeDir.x, 0f, fleeDir.z));
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                10f * Time.deltaTime
            );
        }

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
        if (CurrentState != AnimalState.Flee)
            CurrentState = AnimalState.Flee;
        // Debug.Log("데미지 받음");
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
                Agent.ResetPath();
                break;
        }
    }

    protected override void SetAnimatorController()
    {
        Animator.runtimeAnimatorController = PassiveAsset.AnimatorController;
    }
}
