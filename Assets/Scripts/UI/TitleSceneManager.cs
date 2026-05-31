using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;

    private void Start()
    {
        SaveLoadManager.Init();
        continueButton.interactable = SaveLoadManager.HasSaveData();
    }

    public void OnContinueButton()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnNewGameButton()
    {
        SaveLoadManager.DeleteSave();
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
