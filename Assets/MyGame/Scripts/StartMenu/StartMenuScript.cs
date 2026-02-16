using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StartMenuScript : MonoBehaviour
{
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

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0f);

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSfxVolume(sfxSlider.value);
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

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
