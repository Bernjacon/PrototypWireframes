using System.Collections;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DialoguePersonScripts : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text dialogueText;
    public Image speakerImage;

    [Header("Typing")]
    public float typeSpeed = 0.03f;
    private bool isTyping = false;
    private string currentLine = "";

    [Header("IndexHandling")]
    public int indexDSAArray;

    [Header("Decision")]
    public bool isWaitingForDecision;
    public GameObject showDecisionBox;
    

    public Dialogue[] dsa;

    void Start()
    {
        UpdateObjects();
        showDecisionBox.SetActive(false);
    }

    void Update()
    {
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
                if (dsa[indexDSAArray].isADecision == false)
                {
                    indexDSAArray++;
                    if (indexDSAArray < dsa.Length)
                    {
                        UpdateObjects();
                    }
                    else
                    {
                        Debug.Log("Dialogue finished");
                    }
                }
                else if (dsa[indexDSAArray].isADecision == true)
                {
                    dsa[indexDSAArray].Decision.Invoke();
                }
                
            }
        }
    }

    public void ActivateDecision()
    {
        isWaitingForDecision = true;
        showDecisionBox.SetActive(true);
    }

    public void DecisionWasChosen(int decisionIntInput)
    {
        indexDSAArray = dsa[indexDSAArray].targetIndexAfterDecision[decisionIntInput];
        UpdateObjects();
        showDecisionBox.SetActive(false);
        isWaitingForDecision = false;
    }
    public void UpdateObjects()
    {
        currentLine = dsa[indexDSAArray].textContents;
        speakerImage.sprite = dsa[indexDSAArray].speakerVisual;
        StopAllCoroutines();
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
}

[Serializable]
public class Dialogue
{
    [Header("Dialogue")]
    public string textContents;
    public Sprite speakerVisual;
    [Header("Decision Event")]
    public UnityEvent Decision;
    public bool isADecision;
    public int[] targetIndexAfterDecision;
}

