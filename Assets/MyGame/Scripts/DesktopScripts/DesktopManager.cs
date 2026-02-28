using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using TMPro;

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

    [SerializeField] GameObject[] apps;
    [SerializeField] GameflowActionYield[] gay;

    public void CallEvent(int index) 
    {
        gay[gayIndex].GameStateEvent?.Invoke();
        gayIndex++;
    }
    public void LoginFinished()
    {
        gay[gayIndex].GameStateEvent?.Invoke();
        gayIndex++;
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
        {
            apps[i].SetActive(false);
        }
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
        {
            apps[i].SetActive(false);
        }
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
