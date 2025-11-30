using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Desktop")]
    [SerializeField] private Canvas menu;
    [SerializeField] private Canvas mouseSttg;
    [SerializeField] private Canvas audioSttg;
    [SerializeField] private Canvas b4Start;
    [SerializeField] private Canvas desktopScreen;
    [SerializeField] private GameObject b4StartScreen;
    [SerializeField] private GameObject desktopBG;
    
    [Header("Internetzz")]
    [SerializeField] private Canvas internetzz;
    
    [Header("Galerie")]
    [SerializeField] private Canvas galerie;
    [SerializeField] private GameObject galerieBild;
    [SerializeField] private GameObject galerieAlle;

    private BildID bildID;
    public Sprite[] bild;
    [SerializeField] private Image anzeigeBild;
    private int aktuellerIndex;

    [Header("Videobox")]
    [SerializeField] private Canvas videobox;
    [SerializeField] private GameObject antAusklappen;
    private bool isShown = false;
    
    [Header("Chatbubble")]
    [SerializeField] private Canvas chatbubble;


    private void Start()
    {
        DisableAllCanvas();
        b4Start.enabled = true;
        desktopScreen.enabled = false;
        galerieBild.SetActive(false);
        b4StartScreen.SetActive(true);
        desktopBG.SetActive(false);
        antAusklappen.SetActive(false);
    }
    private void Update()
    {
        anzeigeBild.sprite = bild[aktuellerIndex];
    }

    private void DisableAllCanvas()
    {
        b4Start.enabled = false;
        menu.enabled = false;
        mouseSttg.enabled = false;
        audioSttg.enabled = false;
        internetzz.enabled = false;
        galerie.enabled = false;
        galerieBild.SetActive(false);
        videobox.enabled = false;
        chatbubble.enabled = false;
    }

    public void Bildschirm()
    {
        b4StartScreen.SetActive(false);
        desktopBG.SetActive(true);
        DisableAllCanvas();
        desktopScreen.enabled = true;
    }

    public void Menu()
    {
        DisableAllCanvas();
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

    public void ExitWindow()
    {
        DisableAllCanvas();
    }

    public void Internetzz()
    {
        DisableAllCanvas();
        internetzz.enabled=true;
    }

    public void Galerie() 
    {
       
        DisableAllCanvas();
        galerie.enabled=true;
        galerieBild.SetActive(false);
        galerieAlle.SetActive(true);
        
    }

    public void GalerieDetailAnsicht()
    {

        galerieBild.SetActive(true);
        galerieAlle.SetActive(false);

        bildID = GetComponent<BildID>();
        aktuellerIndex = bildID.id;
    }

    public void NextBild()
    {
        aktuellerIndex++;

        if (aktuellerIndex >= bild.Length)
            aktuellerIndex = 0;
        
    }
    public void PreviousBild()
    {
        aktuellerIndex--;

        if (aktuellerIndex <= 0)
            aktuellerIndex = 14;

    }

    public void Videobox()
    {
        DisableAllCanvas();
        videobox.enabled=true;

    }

    public void Ausklappen()
    {
        
        if (!isShown)
        {
            antAusklappen.SetActive(true);
        }
        else
        {
            antAusklappen.SetActive(false);
        }
        
        isShown = !isShown;
    }

    public void Chatbubble()
    {
        DisableAllCanvas();
        chatbubble.enabled=true;
    }

    public void OfflineMeeting()
    {
        SceneManager.LoadScene(2);
    }
}
