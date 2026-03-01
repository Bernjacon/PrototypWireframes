using TMPro;
using UnityEngine;

public class ReplaceAll : MonoBehaviour
{
    void Awake()
    {
        ReplaceAllNames();
    }

    public static void ReplaceAllNames()
    {
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>(true);

        foreach (TMP_Text text in texts)
        {
            if (text.text.Contains("{playerName}"))
            {
                text.text = text.text.Replace("{playerName}", LoginScript.PlayerName);
            }
        }
    }
}