using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class DesktopManager : MonoBehaviour
{
    [SerializeField] GameObject desktopRootParent;
    [SerializeField] int gayIndex;

    [Header("OpenChatBubbleDownloadWindow")]
    [SerializeField] GameObject internetzMainscreenParent;
    [SerializeField] GameObject chatBubbleDownloadParent;
    [SerializeField] TMP_InputField internetzSuchleiste;
    [SerializeField] string chatBubbleDonwloadLink;
    [SerializeField] GameObject chatBubbleLoginManager;

    [Header("ChatBubbleDownload")]
    [SerializeField] GameObject chatBubbleDownloadWindow;
    [SerializeField] GameObject chatBubbleDownloadFinished;
    [SerializeField] int downloadTime;

    [Header("Artikel Windows")]
    [SerializeField] private GameObject artikel1Parent;
    [SerializeField] private GameObject artikel2Parent;

    [Header("Artikel Completion Flags")]
    [SerializeField] private bool artikelOneCompleted = false;
    [SerializeField] private bool artikelTwoCompleted = false;

    [Header("Chat Input Fields")]
    [SerializeField] private TMP_InputField inputFieldOne;
    [SerializeField] private TMP_InputField inputFieldTwo;
    [SerializeField] private Button sendButtonOne;
    [SerializeField] private Button sendButtonTwo;

    [Header("Preset Messages")]
    [SerializeField] private string presetMessageOne = "apple";
    [SerializeField] private string presetMessageTwo = "banana";

    [Header("Chat Prefab & Container")]
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform contentParentOne;
    [SerializeField] private Transform contentParentTwo;
    [SerializeField] private Sprite playerProfileImage;
    [SerializeField] GameObject[] apps;
    [SerializeField] GameflowActionYield[] gay;

    private int typedIndexOne = 0;
    private int typedIndexTwo = 0;

    private void Start()
    {
        sendButtonOne.interactable = false;
        sendButtonTwo.interactable = false;

        inputFieldOne.onValueChanged.AddListener(HandleTypingOne);
        inputFieldTwo.onValueChanged.AddListener(HandleTypingTwo);
    }

    private void HandleTypingOne(string currentInput)
    {
        if (typedIndexOne >= presetMessageOne.Length)
        {
            inputFieldOne.text = presetMessageOne;
            return;
        }
        inputFieldOne.text = presetMessageOne.Substring(0, typedIndexOne + 1);
        typedIndexOne++;
        if (typedIndexOne >= presetMessageOne.Length)
            sendButtonOne.interactable = true;
        inputFieldOne.caretPosition = inputFieldOne.text.Length;
    }

    private void HandleTypingTwo(string currentInput)
    {
        if (typedIndexTwo >= presetMessageTwo.Length)
        {
            inputFieldTwo.text = presetMessageTwo;
            return;
        }
        inputFieldTwo.text = presetMessageTwo.Substring(0, typedIndexTwo + 1);
        typedIndexTwo++;
        if (typedIndexTwo >= presetMessageTwo.Length)
            sendButtonTwo.interactable = true;
        inputFieldTwo.caretPosition = inputFieldTwo.text.Length;
    }

    public void SendMessageOne()
    {
        if (string.IsNullOrWhiteSpace(inputFieldOne.text))
            return;
        GameObject msgObj = Instantiate(messagePrefab, contentParentOne);
        SetupMessagePrefab(msgObj, LoginScript.PlayerName, inputFieldOne.text, playerProfileImage);
        inputFieldOne.text = "";
        typedIndexOne = 0;
        sendButtonOne.interactable = false;
        inputFieldOne.interactable = false;
        ScrollToBottom(contentParentOne);
        Artikel1Complited();
        CallEvent(6);
    }

    public void SendMessageTwo()
    {
        if (string.IsNullOrWhiteSpace(inputFieldTwo.text))
            return;
        GameObject msgObj = Instantiate(messagePrefab, contentParentTwo);
        SetupMessagePrefab(msgObj, LoginScript.PlayerName, inputFieldTwo.text, playerProfileImage);
        inputFieldTwo.text = "";
        typedIndexTwo = 0;
        sendButtonTwo.interactable = false;
        inputFieldTwo.interactable = false;
        ScrollToBottom(contentParentTwo);
        Artikel2Complited();
        CallEvent(8);

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

    private void ScrollToBottom(Transform contentParent)
    {
        Canvas.ForceUpdateCanvases();
        ScrollRect scrollRect = contentParent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 0f;
    }

    public void Artikel1Complited()
    {
        artikelOneCompleted = true;
        CheckArtikelCompletion();
    }

    public void Artikel2Complited()
    {
        artikelTwoCompleted = true;
        CheckArtikelCompletion();
    }

    private void CheckArtikelCompletion()
    {
        if (artikelOneCompleted && artikelTwoCompleted)
            CallEvent(9);
    }

    public void OpenArtikelOne()
    {
        if (artikel1Parent != null)
            artikel1Parent.SetActive(true);
    }

    public void OpenArtikelTwo()
    {
        if (artikel2Parent != null)
            artikel2Parent.SetActive(true);
    }

    public void CallEvent(int index)
    {
        gay[index].GameStateEvent?.Invoke();
        gayIndex++;
    }

    public void LoginFinished()
    {
        desktopRootParent.SetActive(true);
        gay[gayIndex].GameStateEvent?.Invoke();
        gayIndex++;
        gay[gayIndex].GameStateEvent?.Invoke();
    }

    public void ChatBubbleDownloadWindow()
    {
        StartCoroutine(ChatBubbleDownloadWindowCoroutine());
    }

    public IEnumerator ChatBubbleDownloadWindowCoroutine()
    {
        chatBubbleDownloadWindow.SetActive(true);
        yield return new WaitForSeconds(downloadTime);
        chatBubbleDownloadFinished.SetActive(true);
    }

    public void OpenChatBubbleFirstTime()
    {
        gay[gayIndex].GameStateEvent?.Invoke();
        gayIndex++;
        for (int i = 0; i < apps.Length - 1; i++)
            apps[i].SetActive(false);
        apps[3].SetActive(true);
        chatBubbleLoginManager.SetActive(true);
    }

    public void CloseVideoBox()
    {
        gay[gayIndex].GameStateEvent?.Invoke();
        gayIndex++;
        StartCoroutine(OpenChatbubbleDownload(gayIndex));
    }

    public IEnumerator OpenChatbubbleDownload(int activateInternetzz)
    {
        apps[0].SetActive(true);
        for (int i = 1; i < apps.Length; i++)
            apps[i].SetActive(false);
        yield return new WaitForSeconds(1);
        internetzSuchleiste.text = chatBubbleDonwloadLink;
        yield return new WaitForSeconds(1);
        internetzMainscreenParent.SetActive(false);
        chatBubbleDownloadParent.SetActive(true);
        gay[activateInternetzz].GameStateEvent?.Invoke();
        gayIndex++;
    }

    public void ExitWindow(int appsarrayindex)
    {
        apps[appsarrayindex].SetActive(false);
    }

    public void OpenWindow(int index)
    {
        if (index < 0 || index >= apps.Length)
            return;
        bool isAlreadyOpen = apps[index].activeSelf;
        for (int i = 0; i < apps.Length; i++)
            apps[i].SetActive(false);
        if (!isAlreadyOpen)
            apps[index].SetActive(true);
    }
}

[Serializable]
public class GameflowActionYield
{
    public UnityEvent GameStateEvent;
}