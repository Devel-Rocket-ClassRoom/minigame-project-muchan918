using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("BGM")]
    public AudioSource bgmSource;

    [Header("Clips - BGM")]
    public AudioClip mainBgm;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMainBgm()
    {
        bgmSource.clip = mainBgm;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBgm() => bgmSource.Stop();
}
