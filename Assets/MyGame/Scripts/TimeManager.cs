using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class ClockTMP : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Start()
    {
        StartCoroutine(UpdateClock());
    }

    IEnumerator UpdateClock()
    {
        while (true)
        {
            timeText.text = DateTime.Now.ToString("HH:mm");
            yield return new WaitForSeconds(60);
        }
    }
}
