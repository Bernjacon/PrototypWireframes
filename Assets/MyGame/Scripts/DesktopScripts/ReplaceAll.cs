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
        TMP_Text[] texts = Resources.FindObjectsOfTypeAll<TMP_Text>();

        foreach (TMP_Text text in texts)
        {
            if (text.text.Contains("{playerName}"))
            {
                text.text = text.text.Replace("{playerName}", LoginScript.PlayerName);
            }
        }
    }
}