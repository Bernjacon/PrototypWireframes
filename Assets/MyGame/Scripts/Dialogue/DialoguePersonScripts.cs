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
    [Header("UI")]
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Image speakerImage;
    [SerializeField] Sprite lastSpeakerSprite;
    [SerializeField] TMP_Text timeText;
    [SerializeField] Animator speakerAnimator;

    [Header("Typing")]
    [SerializeField] float typeSpeed = 0.03f;
    bool isTyping = false;
    string currentLine = "";

    [Header("IndexHandling")]
    [SerializeField] int indexDSAArray;

    [Header("Decision")]
    [SerializeField] bool isWaitingForDecision;
    [SerializeField] GameObject[] showDecisionButtons;
    int buttonIndex;
    bool dialogueFinished = false;

    [Header("Audio")]
    private List<GameObject> dialogueAudioObjects = new List<GameObject>();
    private List<GameObject> persistentAudioObjects = new List<GameObject>();

    [SerializeField] Dialogue[] dsa;

    void Start()
    {

        foreach (GameObject btn in showDecisionButtons)
        {
            if (btn != null)
                btn.SetActive(false);
        }

        UpdateObjects();
        StartCoroutine(UpdateClock());
    }

    void Update()
    {
        if (!dialogueFinished && indexDSAArray >= dsa.Length)
        {
            dialogueFinished = true;
            dsa[indexDSAArray].eventVariable.Invoke();
            return;
        }

        if (dialogueFinished) return;

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
                if (!dsa[indexDSAArray].causesEvent)
                {
                    indexDSAArray++;
                    if (indexDSAArray < dsa.Length)
                        UpdateObjects();
                    else
                        Debug.Log("Dialogue finished");
                }
                else
                {
                    dsa[indexDSAArray].eventVariable.Invoke();
                }
            }
        }
    }

    public void ActivateDecision()
    {
        isWaitingForDecision = true;
        buttonIndex = dsa[indexDSAArray].targetIndexAfterDecision.Length;
        for (int i = 0; i < showDecisionButtons.Length; i++)
            showDecisionButtons[i].SetActive(i < buttonIndex);
    }

    public void DecisionWasChosen(int decisionIntInput)
    {
        indexDSAArray = dsa[indexDSAArray].targetIndexAfterDecision[decisionIntInput];
        UpdateObjects();
        buttonIndex = dsa[indexDSAArray].targetIndexAfterDecision.Length;
        for (int i = 0; i < showDecisionButtons.Length; i++)
            showDecisionButtons[i].SetActive(i < buttonIndex);
        isWaitingForDecision = false;
    }

    void UpdateObjects()
    {
        StopAndClearDialogueAudio();

        currentLine = dsa[indexDSAArray].textContents;

        if (dsa[indexDSAArray].speakerVisual != null)
            lastSpeakerSprite = dsa[indexDSAArray].speakerVisual;

        speakerImage.sprite = lastSpeakerSprite;
        if (speakerAnimator != null)
            speakerAnimator.runtimeAnimatorController =
                dsa[indexDSAArray].animation;

        StopAllCoroutines();
        StartCoroutine(TypeText(currentLine));
        PlayDialogueAudio();
        PlayPersistentAudio();
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

    IEnumerator UpdateClock()
    {
        while (true)
        {
            timeText.text = DateTime.Now.ToString("HH:mm");
            yield return new WaitForSeconds(1f);
        }
    }

    void PlayDialogueAudio()
    {
        if (dsa[indexDSAArray].audioClips == null || dsa[indexDSAArray].audioClips.Length == 0)
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
        if (dsa[indexDSAArray].persistentAudioClips == null || dsa[indexDSAArray].persistentAudioClips.Length == 0)
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

}

[Serializable]
public class Dialogue
{
    [Header("Dialogue and Animation")]
    public string textContents;
    public Sprite speakerVisual;
    public RuntimeAnimatorController animation;

    [Header("Audio")]
    public AudioClip[] audioClips;
    public AudioMixerGroup[] audioMixerGroups;

    [Header("Persistent Audio")]
    public AudioClip[] persistentAudioClips;
    public AudioMixerGroup[] persistentAudioMixers;

    [Header("Event")]
    public UnityEvent eventVariable;
    public bool causesEvent;
    public int[] targetIndexAfterDecision;
}