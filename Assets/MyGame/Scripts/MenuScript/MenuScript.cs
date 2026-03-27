using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuScript : MonoBehaviour
{
    public static bool SuppressClick;

    [Header("Panels")]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject settingsGraphic;
    [SerializeField] GameObject menuGraphics;
    [SerializeField] GameObject audioSettings;
    [SerializeField] GameObject startScreenMenu;
    [SerializeField] GameObject audioBTTN;
    [SerializeField] Sprite[] bttnvisual;
    [SerializeField] GameObject[] deactivateGameManagers;

    [Header("Audio Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        menu.SetActive(false);
        settingsGraphic.SetActive(false);
        menuGraphics.SetActive(false);
        audioSettings.SetActive(false);

        if (startScreenMenu != null)
            startScreenMenu.SetActive(false);

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

    public void OpenMenu()
    {
        SuppressClickForOneFrame();

        menu.SetActive(true);
        settingsGraphic.SetActive(true);
        menuGraphics.SetActive(false);
        audioSettings.SetActive(false);

        if (startScreenMenu != null)
            startScreenMenu.SetActive(false);

        deactivateGameManagers[0].SetActive(false);
        deactivateGameManagers[1].GetComponent<DialoguePersonScripts>().enabled = false;
    }

    public void Back()
    {
        SuppressClickForOneFrame();

        if (audioSettings.activeSelf || (startScreenMenu != null && startScreenMenu.activeSelf))
        {
            audioSettings.SetActive(false);

            if (startScreenMenu != null)
                startScreenMenu.SetActive(false);

            menuGraphics.SetActive(false);
            settingsGraphic.SetActive(true);

            if (audioBTTN != null && bttnvisual.Length > 0)
            {
                Image img = audioBTTN.GetComponent<Image>();
                if (img != null)
                    img.sprite = bttnvisual[0];
            }
        }
        else
        {
            menu.SetActive(false);
            settingsGraphic.SetActive(false);
            menuGraphics.SetActive(false);
            audioSettings.SetActive(false);

            if (startScreenMenu != null)
                startScreenMenu.SetActive(false);

            deactivateGameManagers[0].SetActive(true);
            deactivateGameManagers[1].GetComponent<DialoguePersonScripts>().enabled = true;
        }
    }

    public void OpenAudioSettings()
    {
        SuppressClickForOneFrame();

        settingsGraphic.SetActive(false);
        menuGraphics.SetActive(true);
        audioSettings.SetActive(true);

        if (startScreenMenu != null)
            startScreenMenu.SetActive(false);

        if (audioBTTN != null && bttnvisual.Length > 1)
        {
            Image img = audioBTTN.GetComponent<Image>();
            if (img != null)
                img.sprite = bttnvisual[1];
        }
    }

    public void OpenStartScreenMenu()
    {
        SuppressClickForOneFrame();

        settingsGraphic.SetActive(false);
        menuGraphics.SetActive(true);
        audioSettings.SetActive(false);

        if (startScreenMenu != null)
            startScreenMenu.SetActive(true);

        if (audioBTTN != null && bttnvisual.Length > 0)
        {
            Image img = audioBTTN.GetComponent<Image>();
            if (img != null)
                img.sprite = bttnvisual[0];
        }
    }

    public void ResetSetting()
    {
        SuppressClickForOneFrame();

        masterSlider.value = 0f;
        musicSlider.value = 0f;
        sfxSlider.value = 0f;

        SetMasterVolume(0f);
        SetMusicVolume(0f);
        SetSfxVolume(0f);
    }

    public void BackToStartMenu()
    {
        SuppressClickForOneFrame();
        SceneManager.LoadScene(0);
    }

    void SuppressClickForOneFrame()
    {
        SuppressClick = true;
        StartCoroutine(ClearClickSuppression());
    }

    IEnumerator ClearClickSuppression()
    {
        yield return null;
        SuppressClick = false;
    }
}