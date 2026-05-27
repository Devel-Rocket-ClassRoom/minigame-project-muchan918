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
        Vector3 fleePos = transform.position + fleeDir * 10f;

        if (NavMesh.SamplePosition(fleePos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            Agent.SetDestination(hit.position);
        else
            Agent.SetDestination(transform.position + fleeDir * 3f); // 실패 시 짧게라도 이동

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

        fleeTimer -= Time.deltaTime;
        if (fleeTimer <= 0f)
        {
            Agent.ResetPath();
            ResetStateTimer();
            CurrentState = AnimalState.Idle;
        }
    }

    protected override void OnTakeDamage(Vector3 hitNormal)
    {
        fleeTimer = PassiveAsset.FleeDuration;
        if (CurrentState != AnimalState.Flee)
            CurrentState = AnimalState.Flee;
        Debug.Log("데미지 받음");
    }

    protected override void OnStateChanged(AnimalState newState)
    {
        switch (newState)
        {
            case AnimalState.Idle:
                Animator.SetInteger("State", 0);
                Agent.ResetPath();
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
