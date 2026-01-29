using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialoguePersonScripts : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text dialogueText;
    public Image[] speakerImages;        // [0] = NPC, [1] = Player
    public RectTransform[] speakerRects; // same order
    public GameObject choicePanel;
    public TMP_Text choiceA;
    public TMP_Text choiceB;

    [Header("Dialogue Data")]
    [TextArea(2, 5)] public string[] npcDialogue;   // All NPC lines in order
    public string[] playerChoicesA;                 // 1 per cycle
    public string[] playerChoicesB;                 // 1 per cycle

    [Header("Settings")]
    public float typeSpeed = 0.03f;
    public float speakingScale = 1f;
    public float idleScale = 0.85f;
    public float speakingAlpha = 1f;
    public float idleAlpha = 0.5f;

    private int currentCycle = 0;      // 0..3 for 4 cycles
    private int lineInCycle = 0;       // 0..5, tracks which line of NPC we are on
    private bool typing = false;
    private bool waitingForChoice = false;
    private string currentLine = "";

    private Vector3[] originalScales;

    void Start()
    {
        choicePanel.SetActive(false);

        // store original scales
        originalScales = new Vector3[speakerRects.Length];
        for (int i = 0; i < speakerRects.Length; i++)
            originalScales[i] = speakerRects[i].localScale;

        ShowNextNPCLine();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (typing)
            {
                StopAllCoroutines();
                dialogueText.text = currentLine;
                typing = false;
                return;
            }

            if (waitingForChoice) return;

            ShowNextNPCLine();
        }
    }

    // -----------------------------
    // Show next NPC line
    // -----------------------------
    void ShowNextNPCLine()
    {
        int cycleStartIndex = currentCycle * 6; // each cycle has 6 NPC lines: 0-5,6-11,12-17,18-23

       
        if (lineInCycle >= 6)
        {
            currentCycle++;
            lineInCycle = 0;

            if (currentCycle >= playerChoicesA.Length || cycleStartIndex + 6 >= npcDialogue.Length)
                return;
            cycleStartIndex = currentCycle * 6;
        }

        // Pre-choice 3 lines: lineInCycle 0,1,2
        if (lineInCycle < 3)
        {
            SetSpeaker(0); // NPC
            currentLine = npcDialogue[cycleStartIndex + lineInCycle];
            StartCoroutine(TypeText(currentLine));
            lineInCycle++;

            // After third line, show choice
            if (lineInCycle == 3)
            {
                waitingForChoice = true;
                ShowChoice();
            }
        }
        else // Post-choice 3 lines: lineInCycle 3,4,5
        {
            SetSpeaker(0); // NPC
            currentLine = npcDialogue[cycleStartIndex + lineInCycle];
            StartCoroutine(TypeText(currentLine));
            lineInCycle++;
        }
    }

    // -----------------------------
    // Player choice
    // -----------------------------
    void ShowChoice()
    {
        choicePanel.SetActive(true);
        choiceA.text = playerChoicesA[currentCycle];
        choiceB.text = playerChoicesB[currentCycle];
    }

    public void ChooseA()
    {
        HandlePlayerChoice(playerChoicesA[currentCycle]);
    }

    public void ChooseB()
    {
        HandlePlayerChoice(playerChoicesB[currentCycle]);
    }

    void HandlePlayerChoice(string chosenText)
    {
        choicePanel.SetActive(false);
        waitingForChoice = false;

        SetSpeaker(1); // Player speaking
        currentLine = chosenText;
        StartCoroutine(TypeText(currentLine));

        // After choice, next click will show NPC post-choice lines (lineInCycle 3..5)
        // lineInCycle is already 3 after pre-choice lines
    }

    // -----------------------------
    // Typewriter effect
    // -----------------------------
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

    // -----------------------------
    // Speaker visuals
    // -----------------------------
    void SetSpeaker(int speaking)
    {
        for (int i = 0; i < speakerImages.Length; i++)
        {
            // Scale
            float scale = (i == speaking) ? speakingScale : idleScale;
            speakerRects[i].localScale = originalScales[i] * scale;

            // Opacity
            float alpha = (i == speaking) ? speakingAlpha : idleAlpha;
            Color c = speakerImages[i].color;
            c.a = alpha;
            speakerImages[i].color = c;
        }
    }
}
