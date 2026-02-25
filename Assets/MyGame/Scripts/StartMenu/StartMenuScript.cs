using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class StartMenuScript : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainScreenParent;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject settingsGraphic;
    [SerializeField] GameObject menuGraphics;
    [SerializeField] GameObject audioSettings;
    [SerializeField] GameObject audioBTTN;
    [SerializeField] Sprite[] bttnvisual;

    [Header("Audio Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        mainScreenParent.SetActive(true);
        settings.SetActive(false);
        settingsGraphic.SetActive(false);
        menuGraphics.SetActive(false);
        audioSettings.SetActive(false);

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
        settings.SetActive(true);
        settingsGraphic.SetActive(true);
        menuGraphics.SetActive(false);
        audioSettings.SetActive(false);
    }


    public void Back()
    {
        if (audioSettings.activeSelf)
        {
            audioSettings.SetActive(false);
            menuGraphics.SetActive(false);

    if (audioBTTN != null && bttnvisual.Length > 0)
    {
        Image img = audioBTTN.GetComponent<Image>();
        if (img != null)
            img.sprite = bttnvisual[0];
    }
        }
        else
        {
            mainScreenParent.SetActive(true);
            settings.SetActive(false);
            settingsGraphic.SetActive(false);
            menuGraphics.SetActive(false);
            audioSettings.SetActive(false);
        }
    }

    public void OpenAudioSettings()
    {
        settingsGraphic.SetActive(false);
        menuGraphics.SetActive(true);
        audioSettings.SetActive(true);

        if (audioBTTN != null && bttnvisual.Length > 1)
        {
            Image img = audioBTTN.GetComponent<Image>();
            if (img != null)
                img.sprite = bttnvisual[1];
        }
    }


    public void ResetSetting()
    {
        masterSlider.value = 0f;
        musicSlider.value = 0f;
        sfxSlider.value = 0f;

        SetMasterVolume(0f);
        SetMusicVolume(0f);
        SetSfxVolume(0f);
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
