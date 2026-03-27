using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.Audio;

public class DialoguePersonScripts : MonoBehaviour
{
    [Header("UI - Dialogue")]
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] SpriteRenderer speakerImage;
    [SerializeField] SpriteRenderer playerImage;
    [SerializeField] SpriteRenderer boxSpriteRenderer;
    [SerializeField] GameObject playerBackground;
    [SerializeField] Animator speakerAnimator;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator boxAnimator;
    [SerializeField] RectTransform menuBlocker;
    [SerializeField] Camera uiCamera;
    [SerializeField] bool hasClock = true;

    [Header("Special Speaker Settings")]
    [SerializeField] GameObject specialSpeakerObject;
    [SerializeField] float offsetY = -100;
    GameObject lastSpecialSpeaker = null;
    float baseYPosition = 0f;

    [Header("Player Settings")]
    [SerializeField] Sprite playerVisual;
    [SerializeField] RuntimeAnimatorController playerAnimation;

    [Header("Typing")]
    [SerializeField] float typeSpeed = 0.03f;
    private bool isTyping = false;
    private string currentLine = "";
    Coroutine typingCoroutine;
    Coroutine boxAnimationCoroutine;
    Coroutine pauseCoroutine;

    [Header("Dialogue State")]
    [SerializeField] int indexDSAArray;
    private bool dialogueFinished = false;
    private Sprite lastSpeakerSprite;
    private Sprite lastBoxSprite;
    private GameObject lastDisappearingSpeaker;

    [Header("Decision")]
    [SerializeField] bool isWaitingForDecision;
    [SerializeField] GameObject[] showDecisionButtons;

    [Header("Clock")]
    private DateTime simulatedTime;

    [Header("Audio")]
    private List<GameObject> dialogueAudioObjects = new List<GameObject>();
    private List<GameObject> persistentAudioObjects = new List<GameObject>();

    [Header("Pause Dialogue")]
    [SerializeField] GameObject dialogueBoxRoot;
    private bool isPausedTemporarily = false;

    [Header("External")]
    public LoadingManagerScript lmsa;
    [SerializeField] Dialogue[] dsa;

    void ActivateScript()
    {
        enabled = true;
    }

    public void PauseDialogueForSeconds(float seconds)
    {
        if (!gameObject.activeInHierarchy) return;

        if (pauseCoroutine != null) StopCoroutine(pauseCoroutine);
        pauseCoroutine = StartCoroutine(PauseDialogueCoroutine(seconds));
    }

    IEnumerator PauseDialogueCoroutine(float seconds)
    {
        isPausedTemporarily = true;
        isWaitingForDecision = true;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (boxAnimationCoroutine != null) StopCoroutine(boxAnimationCoroutine);
        isTyping = false;

        if (dialogueBoxRoot != null) dialogueBoxRoot.SetActive(false);

        yield return new WaitForSeconds(seconds);

        indexDSAArray++;

        if (dialogueBoxRoot != null) dialogueBoxRoot.SetActive(true);

        isWaitingForDecision = false;
        isPausedTemporarily = false;

        UpdateObjects();
    }

    void Start()
    {
        foreach (GameObject btn in showDecisionButtons)
            if (btn != null) btn.SetActive(false);

        if (playerImage != null) playerImage.gameObject.SetActive(false);
        if (playerAnimator != null && playerAnimation != null)
            playerAnimator.runtimeAnimatorController = playerAnimation;

        if (playerImage != null && playerVisual != null)
            playerImage.sprite = playerVisual;

        if (speakerImage != null) lastSpeakerSprite = speakerImage.sprite;
        if (boxSpriteRenderer != null) lastBoxSprite = boxSpriteRenderer.sprite;

        simulatedTime = DateTime.Today.AddHours(19).AddMinutes(30);

        UpdateObjects();

        if (hasClock) StartCoroutine(UpdateClock());
    }

    void Update()
    {
        if (MenuScript.SuppressClick) return;
        if (dialogueFinished || isPausedTemporarily) return;
        if (indexDSAArray >= dsa.Length) { dialogueFinished = true; return; }

        if ((Mouse.current?.leftButton.wasPressedThisFrame ?? false) && !isWaitingForDecision && !IsPointerOverMenu())
        {
            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                dialogueText.text = currentLine;
                isTyping = false;
            }
            else
            {
                if (dsa[indexDSAArray].causesEvent)
                    dsa[indexDSAArray].eventVariable?.Invoke();
                else
                {
                    indexDSAArray++;
                    if (indexDSAArray < dsa.Length) UpdateObjects();
                    else dialogueFinished = true;
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

            if (active && currentDialogue.decisionButtonTexts != null && i < currentDialogue.decisionButtonTexts.Length)
            {
                TMP_Text textComponent = showDecisionButtons[i].GetComponentInChildren<TMP_Text>();
                if (textComponent != null) textComponent.text = currentDialogue.decisionButtonTexts[i];
            }
        }
    }

    public void DecisionWasChosen(int decisionIntInput)
    {
        isWaitingForDecision = false;

        if (playerBackground != null) playerBackground.SetActive(true);
        if (playerImage != null) playerImage.gameObject.SetActive(false);

        for (int i = 0; i < showDecisionButtons.Length; i++)
            showDecisionButtons[i].SetActive(false);

        indexDSAArray = dsa[indexDSAArray].targetIndexAfterDecision[decisionIntInput];

        UpdateObjects();
    }

    void UpdateObjects()
    {
        if (indexDSAArray >= dsa.Length) return;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (boxAnimationCoroutine != null) StopCoroutine(boxAnimationCoroutine);

        StopAndClearDialogueAudio();

        currentLine = ReplaceVariables(dsa[indexDSAArray].textContents);
        dialogueText.text = "";

        GameObject newDisappearObj = dsa[indexDSAArray].disapearingSpeaker ?? lastDisappearingSpeaker;
        if (lastDisappearingSpeaker != null && lastDisappearingSpeaker != newDisappearObj)
            lastDisappearingSpeaker.SetActive(true);

        if (newDisappearObj != null) newDisappearObj.SetActive(false);
        lastDisappearingSpeaker = newDisappearObj;

        if (dsa[indexDSAArray].speakerVisual != null) lastSpeakerSprite = dsa[indexDSAArray].speakerVisual;
        if (speakerImage != null && lastSpeakerSprite != null) speakerImage.sprite = lastSpeakerSprite;

        if (dsa[indexDSAArray].boxVisual != null) lastBoxSprite = dsa[indexDSAArray].boxVisual;
        if (boxSpriteRenderer != null && lastBoxSprite != null) boxSpriteRenderer.sprite = lastBoxSprite;

        if (speakerAnimator != null && dsa[indexDSAArray].Speaker != null)
            speakerAnimator.runtimeAnimatorController = dsa[indexDSAArray].Speaker;

        if (boxAnimator != null && dsa[indexDSAArray].boxAnimation != null)
        {
            boxAnimator.runtimeAnimatorController = dsa[indexDSAArray].boxAnimation;
            boxAnimationCoroutine = StartCoroutine(StartTypingAndAudioAfterBoxAnimation(dsa[indexDSAArray]));
        }
        else
        {
            typingCoroutine = StartCoroutine(TypeText(currentLine));
            PlayDialogueAudio(dsa[indexDSAArray]);
            PlayPersistentAudio(dsa[indexDSAArray]);
        }

        if (baseYPosition == 0f && speakerImage != null) baseYPosition = speakerImage.transform.localPosition.y;

        if (lastDisappearingSpeaker == specialSpeakerObject)
        {
            if (lastSpecialSpeaker != specialSpeakerObject)
            {
                Vector3 pos = speakerImage.transform.localPosition;
                pos.y = baseYPosition + offsetY;
                speakerImage.transform.localPosition = pos;
            }
            lastSpecialSpeaker = specialSpeakerObject;
        }
        else
        {
            Vector3 pos = speakerImage.transform.localPosition;
            pos.y = baseYPosition;
            speakerImage.transform.localPosition = pos;
            lastSpecialSpeaker = null;
        }
    }

    IEnumerator StartTypingAndAudioAfterBoxAnimation(Dialogue current)
    {
        if (boxAnimator != null)
        {
            boxAnimator.Rebind();
            boxAnimator.Update(0f);
            yield return null;
            yield return null;

            AnimatorStateInfo state = boxAnimator.GetCurrentAnimatorStateInfo(0);
            float animationLength = state.length;
            if (animationLength > 0f) yield return new WaitForSeconds(animationLength);
        }

        typingCoroutine = StartCoroutine(TypeText(currentLine));
        PlayDialogueAudio(current);
        PlayPersistentAudio(current);
    }

    bool IsPointerOverMenu()
    {
        if (menuBlocker == null || Mouse.current == null) return false;
        return RectTransformUtility.RectangleContainsScreenPoint(menuBlocker, Mouse.current.position.ReadValue(), uiCamera);
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        isTyping = false;
    }

    string ReplaceVariables(string input)
    {
        return input.Replace("{playerName}", LoginScript.PlayerName ?? "Player");
    }

    IEnumerator UpdateClock()
    {
        while (hasClock && gameObject.activeInHierarchy)
        {
            timeText.text = simulatedTime.ToString("HH:mm");
            simulatedTime = simulatedTime.AddSeconds(Time.deltaTime);
            yield return null;
        }
    }

    void PlayDialogueAudio(Dialogue current)
    {
        if (current.audioClips == null) return;

        for (int i = 0; i < current.audioClips.Length; i++)
        {
            AudioClip clip = current.audioClips[i];
            if (clip == null) continue;

            GameObject go = new GameObject("TempDialogueAudio_" + clip.name);
            go.transform.parent = transform;

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = false;

            if (current.audioMixerGroups != null && i < current.audioMixerGroups.Length && current.audioMixerGroups[i] != null)
                source.outputAudioMixerGroup = current.audioMixerGroups[i];

            source.Play();
            dialogueAudioObjects.Add(go);
            Destroy(go, clip.length);
        }
    }

    void PlayPersistentAudio(Dialogue current)
    {
        if (current.persistentAudioClips == null) return;

        for (int i = 0; i < current.persistentAudioClips.Length; i++)
        {
            AudioClip clip = current.persistentAudioClips[i];
            if (clip == null) continue;

            GameObject go = new GameObject("PersistentAudio_" + clip.name);
            go.transform.parent = transform;

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = false;

            if (current.persistentAudioMixers != null && i < current.persistentAudioMixers.Length && current.persistentAudioMixers[i] != null)
                source.outputAudioMixerGroup = current.persistentAudioMixers[i];

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
                if (src != null && src.isPlaying) src.Stop();
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
                if (src != null && src.isPlaying) src.Stop();
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
    public string textContents;
    public Sprite speakerVisual;
    public Sprite boxVisual;
    public GameObject disapearingSpeaker;
    public RuntimeAnimatorController Speaker;
    public RuntimeAnimatorController boxAnimation;
    public AudioClip[] audioClips;
    public AudioMixerGroup[] audioMixerGroups;
    public AudioClip[] persistentAudioClips;
    public AudioMixerGroup[] persistentAudioMixers;
    public bool causesEvent;
    public UnityEvent eventVariable;
    public string[] decisionButtonTexts;
    public int[] targetIndexAfterDecision;
}