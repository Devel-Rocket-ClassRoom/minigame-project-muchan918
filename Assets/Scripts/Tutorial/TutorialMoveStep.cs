using TMPro;
using UnityEngine;

public class TutorialMoveStep : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject guidePanel;
    private bool completed = false;

    private void Start()
    {
        guidePanel.SetActive(true);
    }

    private void Update()
    {
        if (completed || playerMovement == null)
            return;

        if (playerMovement.MoveInput.sqrMagnitude > 0.01f)
        {
            completed = true;
            guidePanel.SetActive(false);
            TutorialSceneManager.Instance.CompleteStep(TutorialStep.Move);
        }
    }
}
