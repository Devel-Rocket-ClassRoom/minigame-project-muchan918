using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void OnBackToMenuButton()
    {
        GamePause.Resume();
        SceneManager.LoadScene("TitleScene");
    }
}
