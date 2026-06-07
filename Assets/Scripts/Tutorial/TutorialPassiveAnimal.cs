using System.Collections;
using UnityEngine;

public class TutorialPassiveAnimal : PassiveAnimal
{
    private bool isDying = false;
    public TutorialStep completeStep = TutorialStep.PassiveAnimal;

    protected override void UpdateChunk() { } // 청크 감지 무시

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
        TutorialSceneManager.Instance.CompleteStep(TutorialStep.PassiveAnimal);
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1f);
        EffectManager.Instance.PlayAnimalDie(transform.position);

        Destroy(gameObject);
    }
}
