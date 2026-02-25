using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Animations;

public class DialoguePersonScripts : MonoBehaviour
{
    [Header("UI - Dialogue")]
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text timeText;

    [SerializeField] Image speakerImage;
    [SerializeField] Image playerImage;
    [SerializeField] GameObject playerBackground;

    [SerializeField] Animator speakerAnimator;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator boxAnimator;


    [Header("Player Settings")]
    [SerializeField] Sprite playerVisual;
    [SerializeField] RuntimeAnimatorController playerAnimation;


    [Header("Typing")]
    [SerializeField] float typeSpeed = 0.03f;
    private bool isTyping = false;
    private string currentLine = "";


    [Header("Dialogue State")]
    [SerializeField] int indexDSAArray;
    private bool dialogueFinished = false;

    private Sprite lastSpeakerSprite;
    private GameObject lastDisappearingSpeaker;


    [Header("Decision")]
    [SerializeField] bool isWaitingForDecision;
    [SerializeField] GameObject[] showDecisionButtons;

    private TMP_Text[] decisionButtonTextUI;
    private int buttonIndex;


    [Header("Clock")]
    private DateTime simulatedTime;


    [Header("Audio")]
    private List<GameObject> dialogueAudioObjects = new List<GameObject>();
    private List<GameObject> persistentAudioObjects = new List<GameObject>();


    [Header("External")]
    public LoadingManagerScript lmsa;
    [SerializeField] Dialogue[] dsa;

    void Start()
    {
        foreach (GameObject btn in showDecisionButtons)
            if (btn != null)
                btn.SetActive(false);

        if (playerImage != null)
            playerImage.gameObject.SetActive(false);

        if (playerAnimator != null && playerAnimation != null)
            playerAnimator.runtimeAnimatorController = playerAnimation;

        if (playerImage != null && playerVisual != null)
            playerImage.sprite = playerVisual;

        UpdateObjects();
        simulatedTime = DateTime.Today.AddHours(15);
        StartCoroutine(UpdateClock());
    }

    void Update()
    {
        if (dialogueFinished) return;

        if (indexDSAArray >= dsa.Length)
        {
            dialogueFinished = true;
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !isWaitingForDecision)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLine;
                isTyping = false;
            }
            else
            {
                if (dsa[indexDSAArray].causesEvent)
                {
                    dsa[indexDSAArray].eventVariable?.Invoke();
                }
                else
                {
                    indexDSAArray++;

                    if (indexDSAArray < dsa.Length)
                        UpdateObjects();
                    else
                        dialogueFinished = true;
                }
            }
        }
    }

    public void ActivateDecision()
    {
        isWaitingForDecision = true;

        if (playerImage != null)
        {
            playerBackground.SetActive(false);
            playerImage.gameObject.SetActive(true);
        }

        Dialogue currentDialogue = dsa[indexDSAArray];

        int buttonCount = currentDialogue.targetIndexAfterDecision.Length;

        for (int i = 0; i < showDecisionButtons.Length; i++)
        {
            bool active = i < buttonCount;
            showDecisionButtons[i].SetActive(active);

            if (active &&
                currentDialogue.decisionButtonTexts != null &&
                i < currentDialogue.decisionButtonTexts.Length)
            {
                TMP_Text textComponent =
                    showDecisionButtons[i].GetComponentInChildren<TMP_Text>();

                if (textComponent != null)
                    textComponent.text = currentDialogue.decisionButtonTexts[i];
            }
        }
    }
    public void DecisionWasChosen(int decisionIntInput)
    {
        isWaitingForDecision = false;
        playerBackground.gameObject.SetActive(true);

        if (playerImage != null)
            playerImage.gameObject.SetActive(false);

        for (int i = 0; i < showDecisionButtons.Length; i++)
            showDecisionButtons[i].SetActive(false);

        indexDSAArray = dsa[indexDSAArray].targetIndexAfterDecision[decisionIntInput];
        UpdateObjects();
    }

    void UpdateObjects()
    {
        if (indexDSAArray >= dsa.Length)
            return;

        StopAndClearDialogueAudio();

        currentLine = ReplaceVariables(dsa[indexDSAArray].textContents);

        dialogueText.text = "";

        GameObject newDisappearObj = dsa[indexDSAArray].disapearingSpeaker;
        if (newDisappearObj == null)
            newDisappearObj = lastDisappearingSpeaker;

        if (lastDisappearingSpeaker != null)
            lastDisappearingSpeaker.SetActive(true);

        if (newDisappearObj != null)
            newDisappearObj.SetActive(false);

        lastDisappearingSpeaker = newDisappearObj;

        if (dsa[indexDSAArray].speakerVisual != null)
            lastSpeakerSprite = dsa[indexDSAArray].speakerVisual;

        if (lastSpeakerSprite != null)
            speakerImage.sprite = lastSpeakerSprite;

        if (speakerAnimator != null && dsa[indexDSAArray].Speaker != null)
            speakerAnimator.runtimeAnimatorController = dsa[indexDSAArray].Speaker;

        if (boxAnimator != null && dsa[indexDSAArray].boxAnimation != null)
        {
            boxAnimator.runtimeAnimatorController = dsa[indexDSAArray].boxAnimation;
            StartCoroutine(StartTypingAfterBoxAnimation());
        }
        else
        {
            StartCoroutine(TypeText(currentLine));
        }

        PlayDialogueAudio();
        PlayPersistentAudio();
    }

    IEnumerator StartTypingAfterBoxAnimation()
    {
        if (boxAnimator != null)
        {
            AnimatorStateInfo state = boxAnimator.GetCurrentAnimatorStateInfo(0);

            yield return null;
            state = boxAnimator.GetCurrentAnimatorStateInfo(0);

            float animationLength = state.length;

            yield return new WaitForSeconds(animationLength);
        }

        StartCoroutine(TypeText(currentLine));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }
    string ReplaceVariables(string input)
    {
        input = input.Replace("{playerName}", LoginScript.PlayerName);
        return input;
    }
    IEnumerator UpdateClock()
    {
        while (true)
        {
            timeText.text = simulatedTime.ToString("HH:mm");
            simulatedTime = simulatedTime.AddSeconds(1);
            yield return new WaitForSeconds(1f);
        }
    }

    void PlayDialogueAudio()
    {
        if (dsa[indexDSAArray].audioClips == null)
            return;

        for (int i = 0; i < dsa[indexDSAArray].audioClips.Length; i++)
        {
            AudioClip clip = dsa[indexDSAArray].audioClips[i];
            if (clip == null) continue;

            GameObject go = new GameObject("TempDialogueAudio_" + clip.name);
            go.transform.parent = transform;

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = false;

            if (dsa[indexDSAArray].audioMixerGroups != null &&
                i < dsa[indexDSAArray].audioMixerGroups.Length &&
                dsa[indexDSAArray].audioMixerGroups[i] != null)
            {
                source.outputAudioMixerGroup = dsa[indexDSAArray].audioMixerGroups[i];
            }

            source.Play();

            dialogueAudioObjects.Add(go);
            Destroy(go, clip.length);
        }
    }

    void PlayPersistentAudio()
    {
        if (dsa[indexDSAArray].persistentAudioClips == null)
            return;

        for (int i = 0; i < dsa[indexDSAArray].persistentAudioClips.Length; i++)
        {
            AudioClip clip = dsa[indexDSAArray].persistentAudioClips[i];
            if (clip == null) continue;

            GameObject go = new GameObject("PersistentAudio_" + clip.name);
            go.transform.parent = transform;

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = false;

            if (dsa[indexDSAArray].persistentAudioMixers != null &&
                i < dsa[indexDSAArray].persistentAudioMixers.Length &&
                dsa[indexDSAArray].persistentAudioMixers[i] != null)
            {
                source.outputAudioMixerGroup = dsa[indexDSAArray].persistentAudioMixers[i];
            }

            source.Play();

            persistentAudioObjects.Add(go);
            Destroy(go, clip.length);
        }
    }

    void StopAndClearDialogueAudio()
    {
        foreach (GameObject go in dialogueAudioObjects)
        {
            if (go != null)
            {
                AudioSource src = go.GetComponent<AudioSource>();
                if (src != null && src.isPlaying)
                    src.Stop();

                Destroy(go);
            }
        }

        dialogueAudioObjects.Clear();
    }

    public void StopPersistentAudio()
    {
        foreach (GameObject go in persistentAudioObjects)
        {
            if (go != null)
            {
                AudioSource src = go.GetComponent<AudioSource>();
                if (src != null && src.isPlaying)
                    src.Stop();

                Destroy(go);
            }
        }

        persistentAudioObjects.Clear();
    }

    public void PrepareNextScene()
    {
        StartCoroutine(WaitForClickThenLoad());
    }

    IEnumerator WaitForClickThenLoad()
    {
        yield return null;

        yield return new WaitUntil(() => Mouse.current != null);
        yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);

        lmsa.LoadNextScene();
    }
}

[Serializable]
public class Dialogue
{
    [Header("Dialogue and Animation")]
    public string textContents;
    public Sprite speakerVisual;
    public GameObject disapearingSpeaker;
    public RuntimeAnimatorController Speaker;

    [Header("DialogueBoxAnimation")]
    public RuntimeAnimatorController boxAnimation;

    [Header("Audio")]
    public AudioClip[] audioClips;
    public AudioMixerGroup[] audioMixerGroups;

    [Header("Persistent Audio")]
    public AudioClip[] persistentAudioClips;
    public AudioMixerGroup[] persistentAudioMixers;

    [Header("Event")]
    public bool causesEvent;
    public UnityEvent eventVariable;

    [Header("Decision")]
    public string[] decisionButtonTexts;
    public int[] targetIndexAfterDecision;
}