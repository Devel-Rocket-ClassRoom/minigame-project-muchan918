using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private AudioSource bgmSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private AudioClip buttonClick;

    private void Start()
    {
        SaveLoadManager.Init();
        continueButton.interactable = SaveLoadManager.HasSaveData();
        bgmSource.Play();
    }

    public void OnContinueButton()
    {
        StartCoroutine(LoadScene("MainGameScene"));
    }

    public void OnNewGameButton()
    {
        StartCoroutine(LoadScene("MainGameScene", deleteSave: true));
    }

    public void OnQuitButton()
    {
        sfxSource.PlayOneShot(buttonClick);
        Application.Quit();
    }

    private IEnumerator LoadScene(string sceneName, bool deleteSave = false)
    {
        sfxSource.PlayOneShot(buttonClick);
        bgmSource.Stop();
        yield return new WaitForSeconds(buttonClick.length);
        if (deleteSave)
            SaveLoadManager.DeleteSave();
        SceneManager.LoadScene(sceneName);
    }
}
