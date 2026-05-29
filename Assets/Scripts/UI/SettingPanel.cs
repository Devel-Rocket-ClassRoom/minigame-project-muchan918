using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPanel : MonoBehaviour
{
    public void OnClickSetting()
    {
        gameObject.SetActive(true);
        GamePause.Pause();
    }

    public void OnBackToMenuButton()
    {
        GamePause.Resume();
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickCanel()
    {
        gameObject.SetActive(false);
        GamePause.Resume();
    }
}
