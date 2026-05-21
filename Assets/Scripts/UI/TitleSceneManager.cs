using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
