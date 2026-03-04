using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class EndgameScript : MonoBehaviour
{
    [Header("Intro Animation")]
    [SerializeField] GameObject introAnimationObject;
    [SerializeField] float introAnimationDuration = 3f;
    [SerializeField] float delayAfterAnimation = 2f;

    [Header("Credits")]
    [SerializeField] GameObject creditsObject;
    [SerializeField] TMP_Text infoText;

    [Header("Return")]
    [SerializeField] float holdDuration = 1.5f;

    [SerializeField] GameObject articleParent;

    bool creditsStarted = false;
    bool cutsceneFinished = false;
    float holdTimer;

    void Start()
    {
        articleParent.SetActive(false);
        creditsObject.SetActive(false);
        infoText.gameObject.SetActive(false);

        introAnimationObject.SetActive(true);

        creditsStarted = false;
        cutsceneFinished = false;
        holdTimer = 0f;

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(introAnimationDuration);
        yield return new WaitForSeconds(delayAfterAnimation);
        introAnimationObject.SetActive(false);
        articleParent.SetActive(true);
    }

    void Update()
    {
        HandleMouseStart();
        HandleReturnHold();
    }

    void HandleMouseStart()
    {
        if (creditsStarted || !articleParent.activeSelf)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            creditsStarted = true;

            articleParent.SetActive(false);
            creditsObject.SetActive(true);

            infoText.gameObject.SetActive(true);

            cutsceneFinished = true;
        }
    }

    void HandleReturnHold()
    {
        if (!cutsceneFinished)
            return;

        if (Keyboard.current != null && Keyboard.current.enterKey.isPressed)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDuration)
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }
}