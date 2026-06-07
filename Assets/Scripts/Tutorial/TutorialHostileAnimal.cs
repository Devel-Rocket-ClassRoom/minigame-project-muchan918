using System.Collections;
using UnityEngine;

public class TutorialHostileAnimal : HostileAnimal
{
    private bool isDying = false;

    public override int Damage => 3;

    protected override void UpdateChunk() { }

    protected override void Start()
    {
        base.Start();
        SetHp(40);
    }

    public override void Die()
    {
        if (isDying)
            return;
        isDying = true;

        if (Agent.enabled)
            Agent.ResetPath();
        Agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        CurrentState = AnimalState.Die;
        TutorialSceneManager.Instance.CompleteStep(TutorialStep.HostileAnimal);
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1f);
        EffectManager.Instance.PlayAnimalDie(transform.position);

        Destroy(gameObject);
    }
}
