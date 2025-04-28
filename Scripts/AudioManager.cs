using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip backgroundMusic;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip powerUpSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicSource.Play();
    }

    public void PlayCorrectSound()
    {
        sfxSource.PlayOneShot(correctSound);
    }

    public void PlayIncorrectSound()
    {
        sfxSource.PlayOneShot(incorrectSound);
    }

    public void PlayPowerUpSound()
    {
        sfxSource.PlayOneShot(powerUpSound);
    }

    public void UpdateVolumes()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}