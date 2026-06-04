using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField]
    private TutorialManager tutorialManager;

    [Header("BGM")]
    [SerializeField]
    private Slider bgmSlider;

    [SerializeField]
    private Toggle bgmToggle;

    [SerializeField]
    private GameObject bgmBackgroundOn;

    [SerializeField]
    private GameObject bgmBackgroundOff;

    [Header("SFX")]
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Toggle sfxToggle;

    [SerializeField]
    private GameObject sfxBackgroundOn;

    [SerializeField]
    private GameObject sfxBackgroundOff;

    private void Start()
    {
        bgmSlider.value = SoundManager.Instance.bgmSource.volume;
        sfxSlider.value = SoundManager.Instance.sfxSource.volume;

        bgmToggle.isOn = true;
        sfxToggle.isOn = true;

        UpdateBgmUI(true);
        UpdateSfxUI(true);

        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        bgmToggle.onValueChanged.AddListener(OnBgmToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
    }

    private void OnBgmSliderChanged(float value)
    {
        if (bgmToggle.isOn)
            SoundManager.Instance.bgmSource.volume = value;
    }

    private void OnSfxSliderChanged(float value)
    {
        if (sfxToggle.isOn)
        {
            SoundManager.Instance.sfxSource.volume = value;
            SoundManager.Instance.footstepSource.volume = value;
        }
    }

    private void OnBgmToggleChanged(bool isOn)
    {
        SoundManager.Instance.bgmSource.volume = isOn ? bgmSlider.value : 0f;
        UpdateBgmUI(isOn);
    }

    private void OnSfxToggleChanged(bool isOn)
    {
        SoundManager.Instance.sfxSource.volume = isOn ? sfxSlider.value : 0f;
        SoundManager.Instance.footstepSource.volume = isOn ? sfxSlider.value : 0f;
        UpdateSfxUI(isOn);
    }

    private void UpdateBgmUI(bool isOn)
    {
        bgmBackgroundOn.SetActive(isOn);
        bgmBackgroundOff.SetActive(!isOn);
    }

    private void UpdateSfxUI(bool isOn)
    {
        sfxBackgroundOn.SetActive(isOn);
        sfxBackgroundOff.SetActive(!isOn);
    }

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

    public void OnClickTutorial()
    {
        gameObject.SetActive(false);
        tutorialManager.Show();
    }
}
