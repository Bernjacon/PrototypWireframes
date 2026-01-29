using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiscordSimulation : MonoBehaviour
{
    // ======================
    // LOADING + LOGIN
    // ======================

    [Header("Loading / Login Panels")]
    public GameObject loadingPanel;
    public GameObject loginPanel;
    public GameObject invitePanel;
    public GameObject serverPanel;

    [Header("Login Inputs")]
    public TMP_InputField usernameInput;
    public TMP_InputField pass1Input;
    public TMP_InputField pass2Input;

    public Button registerButton;

    // ======================
    // PANELS (channels)
    // ======================

    [Header("Channel Panels")]
    public GameObject memeChannelPanel;
    public GameObject textChannelPanel;
    public GameObject meetingChannelPanel; // unlocked after dialog end

    [Header("Channel Buttons")]
    public GameObject memeButton;
    public GameObject textButton;
    public GameObject meetingButton; // appears later

    // ======================
    // TEXT CHAT SYSTEM
    // ======================

    [Header("Text Chat Components")]
    public Transform textContent;        // ScrollView -> Viewport -> Content
    public GameObject npcMessagePrefab;  // TMP text inside
    public GameObject playerMessagePrefab;

    [Header("NPC Dialogue")]
    public string[] npcMessages; // All messages in one array

    private int messageIndex = 0;
    private bool dialogActive = false;

    // ======================
    // CHOICE SYSTEM
    // ======================

    [Header("Choice Panel")]
    public GameObject choicePanel;
    public Button[] choiceButtons;     // always 3 buttons

    [Header("Choice Sets (3 choices each)")]
    public string[] choiceSet1;
    public string[] choiceSet2;
    public string[] choiceSet3;

    public int choiceInterval = 5;     // show choices every 5 NPC messages

    // ======================
    // UNITY START
    // ======================

    void Start()
    {
        // hide all panels at start
        loginPanel.SetActive(false);
        invitePanel.SetActive(false);
        serverPanel.SetActive(false);
        choicePanel.SetActive(false);

        memeChannelPanel.SetActive(false);
        textChannelPanel.SetActive(false);
        meetingChannelPanel.SetActive(false);

        meetingButton.SetActive(false); // hidden until dialog ends

        registerButton.interactable = false;

        // Start loading screen
        Invoke("ShowLogin", 3f);

        // Ensure password fields show dots and are limited to 8 characters
        pass1Input.contentType = TMP_InputField.ContentType.Password;
        pass2Input.contentType = TMP_InputField.ContentType.Password;
        pass1Input.characterLimit = 8;
        pass2Input.characterLimit = 8;

        // validation watchers
        usernameInput.onValueChanged.AddListener(delegate { ValidateLogin(); });
        pass1Input.onValueChanged.AddListener(delegate { ValidateLogin(); });
        pass2Input.onValueChanged.AddListener(delegate { ValidateLogin(); });
    }

    // ======================
    // LOADING → LOGIN
    // ======================

    void ShowLogin()
    {
        loadingPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    // ======================
    // LOGIN LOGIC
    // ======================

    void ValidateLogin()
    {
        bool userOK = usernameInput.text.Length > 0;
        bool p1OK = pass1Input.text.Length > 0 && pass1Input.text.Length <= 8;
        bool p2OK = pass2Input.text.Length > 0 && pass2Input.text.Length <= 8;

        bool passMatch =
            (pass1Input.text == pass2Input.text) ||
            (pass1Input.text.Length == 2 && pass2Input.text.Length == 2);

        registerButton.interactable = (userOK && p1OK && p2OK && passMatch);

    }

    public void Register()
    {
        loginPanel.SetActive(false);
        invitePanel.SetActive(true);
    }

    // ======================
    // INVITE → SERVER
    // ======================

    public void AcceptInvite()
    {
        invitePanel.SetActive(false);
        serverPanel.SetActive(true);

        OpenMemeChannel(); // default channel
    }

    // ======================
    // CHANNEL SWITCHING
    // ======================

    public void OpenMemeChannel()
    {
        memeChannelPanel.SetActive(true);
        textChannelPanel.SetActive(false);
        meetingChannelPanel.SetActive(false);
    }

    public void OpenTextChannel()
    {
        memeChannelPanel.SetActive(false);
        textChannelPanel.SetActive(true);
        meetingChannelPanel.SetActive(false);

        if (!dialogActive)
        {
            dialogActive = true;
            InvokeRepeating("SpawnNPCMessage", 1f, 3f);
        }
    }

    public void OpenMeetingChannel()
    {
        memeChannelPanel.SetActive(false);
        textChannelPanel.SetActive(false);
        meetingChannelPanel.SetActive(true);
    }

    // ======================
    // TEXT CHAT: NPC MESSAGES
    // ======================

    void SpawnNPCMessage()
    {
        if (messageIndex >= npcMessages.Length)
        {
            CancelInvoke("SpawnNPCMessage");
            meetingButton.SetActive(true);
            return;
        }

        GameObject msg = Instantiate(npcMessagePrefab, textContent);
        msg.GetComponentInChildren<TMP_Text>().text = npcMessages[messageIndex];

        messageIndex++;

        // Trigger choice panel at correct intervals
        if (messageIndex == choiceInterval)
        {
            TriggerChoice(choiceSet1);
        }
        else if (messageIndex == choiceInterval * 2)
        {
            TriggerChoice(choiceSet2);
        }
        else if (messageIndex == choiceInterval * 3)
        {
            TriggerChoice(choiceSet3);
        }
    }

    void TriggerChoice(string[] choiceSet)
    {
        CancelInvoke("SpawnNPCMessage"); // pause NPC messages
        ShowChoiceSet(choiceSet);
    }

    void ShowChoiceSet(string[] set)
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            int index = i;
            TMP_Text txt = choiceButtons[i].GetComponentInChildren<TMP_Text>();
            txt.text = set[i];

            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => PlayerSelectChoice(set[index]));
        }
    }

    void PlayerSelectChoice(string selection)
    {
        GameObject msg = Instantiate(playerMessagePrefab, textContent);
        msg.GetComponentInChildren<TMP_Text>().text = selection;

        choicePanel.SetActive(false);

        // Resume NPC messages safely
        InvokeRepeating("SpawnNPCMessage", 1f, 3f);
    }
}
