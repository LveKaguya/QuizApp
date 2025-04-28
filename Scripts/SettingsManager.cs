using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    void OnDisable()
    {
        SaveSettings();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UpdateVolumes();
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}