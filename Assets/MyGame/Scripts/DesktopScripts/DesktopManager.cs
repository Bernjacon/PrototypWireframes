using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    [SerializeField] private GameObject desktopRootParent;
    [SerializeField] private GameObject[] apps;

    public void LoginFinished()
    {
        desktopRootParent.SetActive(true);

        for (int i = 0; i < apps.Length; i++)
            apps[i].SetActive(false);
    }

    public void OpenWindow(int index)
    {
        if (index < 0 || index >= apps.Length)
            return;

        bool isAlreadyOpen = apps[index].activeSelf;

        for (int i = 0; i < apps.Length; i++)
            apps[i].SetActive(false); // close all first

        // if it wasn’t open before, open it
        if (!isAlreadyOpen)
            apps[index].SetActive(true);
    }
}

public enum GameStage
{
    None,
    Intro,
    PCLogin,
    Desktop,
    Puzzle1,
    Puzzle2,
    End
}
