using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class DesktopManager : MonoBehaviour
{
    [SerializeField] GameObject desktopRootParent;
    [SerializeField] GameObject[] apps;
    [SerializeField] GameflowActionYield[] gay;
    [SerializeField] int gayIndex;

    public void LoginFinished()
    {
        gay[gayIndex].OnEnter?.Invoke();
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
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
}
