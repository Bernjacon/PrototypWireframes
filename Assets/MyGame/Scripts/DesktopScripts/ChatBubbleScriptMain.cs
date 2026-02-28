using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChatBubbleScriptMain : MonoBehaviour
{
    [Header("Prefabs & UI")]
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private GameObject messageInvitePrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private Button sendButton;


    [Header("Player Settings")]
    [SerializeField] private Sprite playerProfileImage;
    [SerializeField] TMP_Text playerName;

    [Header("Finish")]
    [SerializeField] GameObject channel2button;

    private int currentMessageIndex = 0;
    private Coroutine conversationRoutine;

    private string lastSpeakerName = "NPC";
    private string lastMessageText = "";
    private Sprite lastProfileImage;

    [Header("Chat Data")]
    [SerializeField] private List<ChatMessageData> cmds;
    private void Start()
    {
        lastProfileImage = playerProfileImage;
        playerInput.interactable = false;
        sendButton.gameObject.SetActive(false);
        playerName.text = LoginScript.PlayerName;

        conversationRoutine = StartCoroutine(ContinueConversation());
    }

    private IEnumerator ContinueConversation()
    {
        while (currentMessageIndex < cmds.Count)
        {
            ChatMessageData data = cmds[currentMessageIndex];

            // Wait BEFORE showing the message
            yield return new WaitForSeconds(data.delayToNextMessage);

            DisplayMessage(data);

            if (data.waitForPlayer)
            {
                data.TriggerPlayerEvent();
                yield break; // Pause conversation until player sends
            }

            currentMessageIndex++;
        }
    }

    private void DisplayMessage(ChatMessageData data)
    {
        string speaker = string.IsNullOrEmpty(data.speakerName) ? lastSpeakerName : data.speakerName;
        string message = string.IsNullOrEmpty(data.messageText) ? lastMessageText : data.messageText;
        Sprite profile = data.profileImage == null ? lastProfileImage : data.profileImage;

        lastSpeakerName = speaker;
        lastMessageText = message;
        lastProfileImage = profile;

        GameObject prefabToUse = data.sendInviteButton && messageInvitePrefab != null
            ? messageInvitePrefab
            : messagePrefab;

        GameObject msgObj = Instantiate(prefabToUse, contentParent);

        SetupMessagePrefab(msgObj, speaker, message, profile);

        if (data.sendInviteButton)
        {
            Button button = msgObj.transform.Find("Button").GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ActivateSecondChanel);
        }
    }

    public void ActivateSecondChanel()
    {
        channel2button.SetActive(true);
    }

    private void SetupMessagePrefab(GameObject prefabObj, string speaker, string message, Sprite profile)
    {
        TMP_Text nameTMP = prefabObj.transform.Find("Username").GetComponent<TMP_Text>();
        nameTMP.text = speaker;

        TMP_Text messageTMP = prefabObj.transform.Find("Txt_Nachricht").GetComponent<TMP_Text>();
        messageTMP.text = message;

        TMP_Text timeTMP = prefabObj.transform.Find("TimeTMP").GetComponent<TMP_Text>();
        timeTMP.text = DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss");

        Image profileImage = prefabObj.transform.Find("PH_UserprofilePicture").GetComponent<Image>();
        profileImage.sprite = profile;
    }

    public void ActivatePlayerInput()
    {
        playerInput.interactable = true;
        playerInput.text = "";
        sendButton.gameObject.SetActive(true);

        sendButton.onClick.RemoveAllListeners();
        sendButton.onClick.AddListener(OnPlayerSend);
    }

    private void OnPlayerSend()
    {
        string inputText = playerInput.text;
        if (string.IsNullOrWhiteSpace(inputText))
            return;

        ChatMessageData playerData = new ChatMessageData
        {
            speakerName = LoginScript.PlayerName,
            messageText = inputText,
            profileImage = playerProfileImage,
            waitForPlayer = false
        };

        DisplayMessage(playerData);

        playerInput.text = "";
        playerInput.interactable = false;
        sendButton.gameObject.SetActive(false);

        currentMessageIndex++;

        // Resume conversation with delay
        if (conversationRoutine != null)
            StopCoroutine(conversationRoutine);

        conversationRoutine = StartCoroutine(ContinueConversation());
    }
}

[Serializable]
public class ChatMessageData
{
    public string speakerName;
    public string messageText;
    public Sprite profileImage;
    public float delayToNextMessage = 2f;
    public bool waitForPlayer = false;

    public bool sendInviteButton = false; // NEU

    public UnityEvent OnReadyForPlayer;

    public void TriggerPlayerEvent()
    {
        if (waitForPlayer)
            OnReadyForPlayer?.Invoke();
    }
}