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
    [SerializeField] private Transform contentParent;
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private Button sendButton;

    [Header("Chat Data")]
    [SerializeField] private List<ChatMessageData> messages;

    [Header("Player Settings")]
    [SerializeField] private Sprite playerProfileImage;

    private int currentMessageIndex = 0;

    private void Start()
    {
        playerInput.interactable = false;
        sendButton.gameObject.SetActive(false);

        // Starte die Konversation
        StartCoroutine(DisplayNextMessage());
    }

    private IEnumerator DisplayNextMessage()
    {
        if (currentMessageIndex >= messages.Count)
            yield break;

        ChatMessageData data = messages[currentMessageIndex];

        // NPC Nachricht Prefab
        GameObject msgObj = Instantiate(messagePrefab, contentParent);
        SetupMessagePrefab(msgObj, data);

        yield return new WaitForSeconds(data.delayToNextMessage);

        if (data.waitForPlayer)
        {
            data.TriggerPlayerEvent(); // UnityEvent wird ausgelöst
        }
        else
        {
            currentMessageIndex++;
            StartCoroutine(DisplayNextMessage());
        }
    }

    private void SetupMessagePrefab(GameObject prefabObj, ChatMessageData data)
    {
        TMP_Text nameTMP = prefabObj.transform.Find("Username").GetComponent<TMP_Text>();
        nameTMP.text = data.speakerName;

        TMP_Text messageTMP = prefabObj.transform.Find("Txt_Nachricht").GetComponent<TMP_Text>();
        messageTMP.text = data.messageText;

        TMP_Text timeTMP = prefabObj.transform.Find("TimeTMP").GetComponent<TMP_Text>();
        timeTMP.text = DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss");
        // Change from SpriteRenderer to Image
        Image profileImage = prefabObj.transform.Find("PH_UserprofilePicture").GetComponent<Image>();
        profileImage.sprite = data.profileImage;
    }

    public void ActivatePlayerInput() // kann direkt als UnityEvent im Inspector zugewiesen werden
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

        GameObject playerMsg = Instantiate(messagePrefab, contentParent);

        ChatMessageData playerData = new ChatMessageData
        {
            speakerName = LoginScript.PlayerName,
            messageText = inputText,
            profileImage = playerProfileImage, // Player image from manager
            waitForPlayer = false
        };

        SetupMessagePrefab(playerMsg, playerData);

        playerInput.text = "";
        playerInput.interactable = false;
        sendButton.gameObject.SetActive(false);

        currentMessageIndex++;
        StartCoroutine(DisplayNextMessage());
    }
}

[Serializable]
public class ChatMessageData
{
    public string speakerName;
    public string messageText;
    public Sprite profileImage; // Only NPC uses this
    public float delayToNextMessage = 2f;
    public bool waitForPlayer = false;

    public UnityEvent OnReadyForPlayer;

    public void TriggerPlayerEvent()
    {
        if (waitForPlayer)
            OnReadyForPlayer?.Invoke();
    }
}