using System.Collections;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialoguePersonScripts : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text dialogueText;
    public Image speakerImage;

    [Header("Typing")]
    public float typeSpeed = 0.03f;
    private bool typing = false;
    private string currentLine = "";

    public int index;
    public Dialogue[] dsa;

    void Start()
    {
        UpdateObjects();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (typing)
            {
                StopAllCoroutines();
                dialogueText.text = currentLine;
                typing = false;
            }
            else
            {
                // Go to next line
                index++;
                if (index < dsa.Length)
                {
                    UpdateObjects();
                }
                else
                {
                    Debug.Log("Dialogue finished");
                }
            }
        }
    }

    public void UpdateObjects()
    {
        currentLine = dsa[index].textContents;
        speakerImage.sprite = dsa[index].speakerVisual;

        StopAllCoroutines();
        StartCoroutine(TypeText(currentLine));
    }

    IEnumerator TypeText(string line)
    {
        typing = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        typing = false;
    }
}

[Serializable]
public class Dialogue
{
    public string textContents;
    public Sprite speakerVisual;
}
