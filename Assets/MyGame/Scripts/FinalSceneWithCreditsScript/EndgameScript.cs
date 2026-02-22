using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class EndgameScript : MonoBehaviour
{
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
        articleParent.SetActive(true);

        creditsObject.SetActive(false);
        infoText.gameObject.SetActive(false);

        creditsStarted = false;
        cutsceneFinished = false;
        holdTimer = 0f;
    }

    void Update()
    {
        HandleMouseStart();
        HandleReturnHold();
    }

    void HandleMouseStart()
    {
        if (creditsStarted)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            creditsStarted = true;

            // Switch from article to credits
            articleParent.SetActive(false);
            creditsObject.SetActive(true);

            // Show info text
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