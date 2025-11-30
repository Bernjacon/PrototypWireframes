using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas menu;
    [SerializeField] private Canvas mouseSttg;
    [SerializeField] private Canvas audioSttg;
    [SerializeField] private Canvas b4Start;
    [SerializeField] private Canvas desktopScreen;
    [SerializeField] private Canvas internetzz;
    [SerializeField] private Canvas galerie;
    [SerializeField] private Canvas galerieBild;
    [SerializeField] private Canvas videobox;
    [SerializeField] private Canvas chatbubble;
    [SerializeField] private Image b4StartScreen;
    [SerializeField] private Image desktopBG;

    private void Start()
    {
        b4Start.enabled = true;
        menu.enabled = false;
        mouseSttg.enabled = false;
        audioSttg.enabled = false;
        desktopScreen.enabled = false;
        internetzz.enabled = false;
        galerie.enabled = false;
        galerieBild.enabled = false;
        videobox.enabled = false;
        chatbubble.enabled = false;
        b4StartScreen.enabled = true;
        desktopBG.enabled = false;
    }

}
