using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class EndgameScript : MonoBehaviour
{
    public static int videoClipIndex;

    [Header("Video")]
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] GameObject videoPlayerGameObject;

    [Header("Credits")]
    [SerializeField] GameObject creditsObject;
    [SerializeField] TMP_Text infoText;

    [Header("Return")]
    [SerializeField] float holdDuration = 1.5f;

    bool cutsceneFinished;
    float holdTimer;

    void Start()
    {
        creditsObject.SetActive(false);
        infoText.gameObject.SetActive(false);
        videoPlayer.loopPointReached += HandleVideoFinished;
        if (videoClipIndex >= 0 && videoClipIndex < videoClips.Length)
            videoPlayer.clip = videoClips[videoClipIndex];
        videoPlayer.Play();
    }
    void HandleVideoFinished(VideoPlayer _)
    {
        videoPlayer.loopPointReached -= HandleVideoFinished;
        creditsObject.SetActive(true);
        infoText.gameObject.SetActive(true);
        videoPlayerGameObject.SetActive(false);
        cutsceneFinished = true;
    }
    void Update()
    {
        if (!cutsceneFinished)
            return;

        if (Keyboard.current != null && Keyboard.current.enterKey.isPressed)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
                SceneManager.LoadScene(0);
        }
        else
        {
            holdTimer = 0f;
        }
    }

}
