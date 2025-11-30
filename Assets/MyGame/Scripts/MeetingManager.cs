using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeetingManager : MonoBehaviour
{
    [SerializeField] private GameObject dialog;
    [SerializeField] private GameObject antworten;
    [SerializeField] private GameObject dialogbox;
    [SerializeField] private GameObject people;

    [Header("Settings")]
    [SerializeField] private Canvas menu;
    [SerializeField] private Canvas mouseSttg;
    [SerializeField] private Canvas audioSttg;

    private void Start()
    {
        Default();
    }
     
    public void Default()
    {
        CloseAllWindows();
        people.SetActive(true);
    }

    private void CloseAllWindows()
    {
        dialog.SetActive(false);
        antworten.SetActive(false);
        dialogbox.SetActive(false);
        menu.enabled = false;
        people.SetActive(false);
    }
    public void Menu()
    {
        CloseAllWindows();
        menu.enabled = true;

    }

    public void MouseSettings()
    {
        mouseSttg.enabled = true;
        audioSttg.enabled = false;
    }

    public void AudioSettings()
    {
        audioSttg.enabled = true;
        mouseSttg.enabled = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Talking()
    {
        CloseAllWindows();
        dialog.SetActive(true);
        dialogbox.SetActive(true);
    }

    public void Antworten()
    {
        CloseAllWindows();
        antworten.SetActive(true);
        dialogbox.SetActive(true);
    }

    

}
