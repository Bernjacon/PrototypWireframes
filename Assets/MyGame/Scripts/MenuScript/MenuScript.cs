using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour
{
    [Header("Menu Root")]
    [SerializeField] GameObject menuParent;

    [Header("Deactivate When Menu Opens")]
    [SerializeField] GameObject[] deactivateGameManagers;

    [Header("Panels")]
    [SerializeField] GameObject mainScreenParent;
    [SerializeField] GameObject settingsParent;
    [SerializeField] GameObject settingsDefault;
    [SerializeField] GameObject audioSettingsParent;

    [Header("Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;
    public void OpenMenu()
    {
        menuParent.SetActive(true);

        foreach (GameObject go in deactivateGameManagers)
        {
            if (go != null)
                go.SetActive(false);
        }

        mainScreenParent.SetActive(true);
        settingsParent.SetActive(false);
        settingsDefault.SetActive(false);
        audioSettingsParent.SetActive(false);
    }
    public void BackToGame()
    {
        menuParent.SetActive(false);

        foreach (GameObject go in deactivateGameManagers)
        {
            if (go != null)
                go.SetActive(true);
        }
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat("SfxVolume", value);
        PlayerPrefs.SetFloat("SfxVolume", value);
    }

    public void OpenSettings()
    {
        mainScreenParent.SetActive(false);
        settingsParent.SetActive(true);
        settingsDefault.SetActive(true);
        audioSettingsParent.SetActive(false);
    }

    public void BackToMainScreen()
    {
        mainScreenParent.SetActive(true);
        settingsParent.SetActive(false);
        settingsDefault.SetActive(false);
        audioSettingsParent.SetActive(false);
    }

    public void OpenAudioSettings()
    {
        settingsDefault.SetActive(false);
        audioSettingsParent.SetActive(true);
    }

    public void BackToSettings()
    {
        settingsDefault.SetActive(true);
        audioSettingsParent.SetActive(false);
    }
}
