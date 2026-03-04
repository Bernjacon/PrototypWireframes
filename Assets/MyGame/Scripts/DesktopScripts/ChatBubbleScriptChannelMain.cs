using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class ChatBubbleScriptChannelMain : MonoBehaviour
{
    [Header("Prefabs & UI")]
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private Transform contentParentSecondStage;

    private bool useSecondStageParent = false;

    [Header("Player Settings")]
    [SerializeField] private Sprite playerProfileImage;
    [SerializeField] private GameObject[] decisionButtons;

    [Header("Audio")]
    public AudioSource benachrichtingungSource;

    [SerializeField] GameObject channel2button;
    [SerializeField] GameObject channel3button;

    [SerializeField] GameObject decsionParent;

    [Header("Chat Data")]
    [SerializeField] private List<ChatMessageDataChannelMain> messagesChannelMain;

    private ChatMessageDataChannelMain currentData;
    private int currentMessageIndex = 0;
    private Coroutine conversationRoutine;
    private bool isPausedForDecision = false;
    private bool isPausedDueToInactive = false;

    bool artikel1WasClicked = false;
    bool artikel2WasClicked = false;
    int miniGameWin;


    private void Start()
    {
        foreach (var btn in decisionButtons)
            btn.SetActive(false);

        if (gameObject.activeInHierarchy)
            conversationRoutine = StartCoroutine(ContinueConversation());
    }

    private void OnDisable()
    {
        if (conversationRoutine != null)
        {
            StopCoroutine(conversationRoutine);
            isPausedDueToInactive = true;
        }
    }

    private void OnEnable()
    {
        if (isPausedDueToInactive)
        {
            conversationRoutine = StartCoroutine(ContinueConversation());
            isPausedDueToInactive = false;
        }
    }

    private IEnumerator ContinueConversation()
    {
        while (currentMessageIndex < messagesChannelMain.Count)
        {
            var data = messagesChannelMain[currentMessageIndex];

            yield return new WaitForSeconds(data.delayToNextMessage);

            DisplayMessage(data);

            if (data.triggerEvent && data.OnEvent != null)
            {
                data.OnEvent.Invoke();
                if (isPausedForDecision)
                    yield break;
            }

            while (data.waitUntilReleased)
            {
                yield return null;
            }

            currentMessageIndex++;
        }
    }

    private void DisplayMessage(ChatMessageDataChannelMain data)
    {
        if (data.benachrichtigungsSound != null)
            benachrichtingungSource.clip = data.benachrichtigungsSound;

        if (benachrichtingungSource.clip != null)
            benachrichtingungSource.Play();

        currentData = data;

        string speaker = string.IsNullOrEmpty(data.speakerName) ? "NPC" : data.speakerName;
        string message = string.IsNullOrEmpty(data.messageText) ? "" : data.messageText;
        Sprite profile = data.profileImage == null ? playerProfileImage : data.profileImage;

        GameObject prefabToUse = (data.useAlternatePrefab && data.alternatePrefab != null)
                                 ? data.alternatePrefab
                                 : messagePrefab;

        Transform targetParent = useSecondStageParent && contentParentSecondStage != null
                                 ? contentParentSecondStage
                                 : contentParent;

        GameObject msgObj = Instantiate(prefabToUse, targetParent);

        TMP_Text nameTMP = msgObj.transform.Find("Username")?.GetComponent<TMP_Text>();
        TMP_Text messageTMP = msgObj.transform.Find("Txt_Nachricht")?.GetComponent<TMP_Text>();
        TMP_Text timeTMP = msgObj.transform.Find("TimeTMP")?.GetComponent<TMP_Text>();
        Image profileImage = msgObj.transform.Find("PH_UserprofilePicture")?.GetComponent<Image>();

        if (nameTMP != null) nameTMP.text = speaker;
        if (messageTMP != null) messageTMP.text = message;
        if (timeTMP != null) timeTMP.text = DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss");
        if (profileImage != null) profileImage.sprite = profile;
    }

    public void ReleasePauseAndSwitchToSecondStage()
    {
        if (currentMessageIndex < messagesChannelMain.Count)
        {
            messagesChannelMain[currentMessageIndex].waitUntilReleased = false;
        }

        useSecondStageParent = true;

        if (conversationRoutine != null)
            StopCoroutine(conversationRoutine);

        conversationRoutine = StartCoroutine(ContinueConversation());
    }

    public void ActivateDecisionCurrent()
    {
        if (currentData != null)
            ActivateDecision(currentData);
    }

    public void ActivateDecision(ChatMessageDataChannelMain data)
    {
        isPausedForDecision = true;

        int buttonCount = Mathf.Min(
            data.decisionOptions?.Length ?? 0,
            data.targetIndexAfterDecision?.Length ?? 0
        );

        decsionParent.SetActive(true);
        for (int i = 0; i < decisionButtons.Length; i++)
        {
            bool active = i < buttonCount;
            decisionButtons[i].SetActive(active);

            if (active)
            {
                TMP_Text buttonText = decisionButtons[i].GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                    buttonText.text = data.decisionOptions[i];

                int capturedIndex = i;
                Button btn = decisionButtons[i].GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => DecisionChosen(capturedIndex, data));
            }
        }
    }

    private void DecisionChosen(int buttonIndex, ChatMessageDataChannelMain data)
    {
        isPausedForDecision = false;
        decsionParent.SetActive(false);
        foreach (var btn in decisionButtons)
            btn.SetActive(false);

        if (data.targetIndexAfterDecision != null &&
            buttonIndex < data.targetIndexAfterDecision.Length &&
            data.targetIndexAfterDecision[buttonIndex] >= 0)
        {
            currentMessageIndex = data.targetIndexAfterDecision[buttonIndex];
        }
        else
        {
            currentMessageIndex++;
        }

        if (conversationRoutine != null)
            StopCoroutine(conversationRoutine);

        conversationRoutine = StartCoroutine(ContinueConversation());
    }

    public void Artikel1WasPressed()
    {
        artikel1WasClicked = true;
        CheckComplition();
    }

    public void Artikel2WasPressed()
    {
        artikel2WasClicked = true;
        CheckComplition();
    }

    public void RegisterMiniGameWin()
    {
        miniGameWin++;
        CheckComplition();
    }

    private void CheckComplition()
    {
        if (artikel1WasClicked
            && artikel2WasClicked
            && miniGameWin >= 2)
        {
            ReleasePauseAndSwitchToSecondStage();
        }

        channel2button.SetActive(true);
        channel3button.SetActive(true);
    }
}

[Serializable]
public class ChatMessageDataChannelMain
{
    public string speakerName;
    public string messageText;
    public Sprite profileImage;
    public float delayToNextMessage = 2f;
    public AudioClip benachrichtigungsSound;

    [Header("Decision / Event")]
    public bool triggerEvent = false;
    public UnityEvent OnEvent;

    [Header("Decision Options")]
    public string[] decisionOptions;
    public int[] targetIndexAfterDecision;

    [Header("Prefab Override")]
    public bool useAlternatePrefab = false;
    public GameObject alternatePrefab;

    [Header("Wait / Pause Control")]
    public bool waitUntilReleased = false;
}