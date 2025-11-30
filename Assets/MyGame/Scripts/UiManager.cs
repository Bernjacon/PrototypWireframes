using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Security.Cryptography.X509Certificates;
public class UiManager : MonoBehaviour
{
    [SerializeField] private Canvas settings;
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas mouseSttg;
    [SerializeField] private Canvas audioSttg;
    private void Start()
    {
        settings.enabled = false;
        mainMenu.enabled = true; 
        mouseSttg.enabled = false;
        audioSttg.enabled = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        settings.enabled=true;
        mainMenu.enabled = false;
        mouseSttg.enabled = false;
        audioSttg.enabled = false;
    }
    
    public void MainMenu()
    {
        settings.enabled = false;
        mainMenu.enabled = true;
        mouseSttg.enabled = false;
        audioSttg.enabled = false;
    }

    public void MouseSettings()
    {
        mouseSttg.enabled=true;
        audioSttg.enabled = false;
    }

    public void AudioSettings()
    {
        audioSttg.enabled = true;
        mouseSttg.enabled = false;
    }

}
