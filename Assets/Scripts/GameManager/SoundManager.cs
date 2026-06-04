using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("BGM")]
    public AudioSource bgmSource;

    [Header("SFX")]
    public AudioSource footstepSource;
    public AudioSource sfxSource;

    [Header("Clips - BGM")]
    public AudioClip mainBgm;

    [Header("Clips - SFX")]
    public AudioClip footstepClip;
    public AudioClip actionClip;

    [Header("Clips - Interaction")]
    public AudioClip interactClip;
    public AudioClip pickUpClip;

    public AudioClip animalHitClip;
    public AudioClip resourceHitClip;
    public AudioClip playerHitClip;
    public AudioClip playerDieClip;
    public AudioClip transitionOpenClip;

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

    public void PlayFootstep()
    {
        if (footstepSource.isPlaying)
            return;
        footstepSource.Play();
    }

    public void StopFootstep()
    {
        if (!footstepSource.isPlaying)
            return;
        footstepSource.Stop();
    }

    public void StopBgm() => bgmSource.Stop();

    public void PlayAction()
    {
        sfxSource.PlayOneShot(actionClip);
    }

    public void PlayInteract()
    {
        sfxSource.PlayOneShot(interactClip);
    }

    public void PlayPickUp()
    {
        sfxSource.PlayOneShot(pickUpClip);
    }

    public void PlayAnimalHit()
    {
        sfxSource.PlayOneShot(animalHitClip);
    }

    public void PlayResourceHit()
    {
        sfxSource.PlayOneShot(resourceHitClip);
    }

    public void PlayPlayerHit()
    {
        sfxSource.PlayOneShot(playerHitClip);
    }

    public void PlayPlayerDie()
    {
        sfxSource.PlayOneShot(playerDieClip);
    }

    public void PlayTransitionOpen()
    {
        sfxSource.PlayOneShot(transitionOpenClip);
    }
}
