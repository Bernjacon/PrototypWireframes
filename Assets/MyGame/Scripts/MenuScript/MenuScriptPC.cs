using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;

public class MenuScriptPC : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject settingsGraphic;
    [SerializeField] GameObject menuGraphics;
    [SerializeField] GameObject audioSettings;
    [SerializeField] GameObject startScreenMenu;
    [SerializeField] GameObject audioBTTN;
    [SerializeField] Sprite[] bttnvisual;

    [Header("Scripts/Objects to Pause")]
    [SerializeField] GameObject[] deactivateGameManagers;
    private bool[] originalActiveStates;

    [Header("Audio Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;

    public static bool SuppressClick;

    private void Start()
    {
        menu.SetActive(false);
        settingsGraphic.SetActive(false);
        menuGraphics.SetActive(false);
        audioSettings.SetActive(false);

        if (startScreenMenu != null)
            startScreenMenu.SetActive(false);

        originalActiveStates = new bool[deactivateGameManagers.Length];
        for (int i = 0; i < deactivateGameManagers.Length; i++)
            originalActiveStates[i] = deactivateGameManagers[i].activeSelf;

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0f);

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSfxVolume(sfxSlider.value);
    }

    public void OpenMenu()
    {
        SuppressClickForOneFrame();

        originalActiveStates = new bool[deactivateGameManagers.Length];
        for (int i = 0; i < deactivateGameManagers.Length; i++)
            originalActiveStates[i] = deactivateGameManagers[i].activeSelf;

        menu.SetActive(true);
        settingsGraphic.SetActive(true);
        menuGraphics.SetActive(false);
        audioSettings.SetActive(false);

        if (startScreenMenu != null)
            startScreenMenu.SetActive(false);

        for (int i = 0; i < deactivateGameManagers.Length; i++)
        {
            if (deactivateGameManagers[i] != null)
            {
                deactivateGameManagers[i].SetActive(false);

                var dialogue = deactivateGameManagers[i].GetComponent<DialoguePersonScripts>();
                if (dialogue != null)
                    dialogue.enabled = false;
            }
        }
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

            for (int i = 0; i < deactivateGameManagers.Length; i++)
            {
                if (deactivateGameManagers[i] != null)
                {
                    deactivateGameManagers[i].SetActive(originalActiveStates[i]);

                    var dialogue = deactivateGameManagers[i].GetComponent<DialoguePersonScripts>();
                    if (dialogue != null)
                        dialogue.enabled = originalActiveStates[i];
                }
            }
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

    public void BackToStartMenu()
    {
        SceneManager.LoadScene(0);
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
}