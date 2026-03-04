using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class DesktopManager : MonoBehaviour
{
    [SerializeField] GameObject desktopRootParent;
    [SerializeField] public int gayIndex;

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

    [SerializeField] TMP_Text clockTMP;

    public GameObject[] apps;
    [SerializeField] GameflowActionYield[] gay;

    public void CallEvent(int index)
    {
        gay[index].GameStateEvent?.Invoke();
        gayIndex++;
    }

    public void Start()
    {
        StartCoroutine(UpdateClock());
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

    IEnumerator UpdateClock()
    {
        while (true)
        {
            clockTMP.text = DateTime.Now.ToString("HH:mm");
            yield return new WaitForSeconds(60f);
        }
    }
}

[Serializable]
public class GameflowActionYield
{
    public UnityEvent GameStateEvent;
}