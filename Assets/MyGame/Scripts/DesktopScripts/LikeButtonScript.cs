using TMPro;
using UnityEngine;

public class LikeButtonScript : MonoBehaviour
{
    [SerializeField] private int likes = 0;

    private TMP_Text targetTMP;
    private bool liked = false;

    void Awake()
    {
        targetTMP = GetComponent<TMP_Text>();
        UpdateText();
    }

    public void ToggleLike()
    {
        if (liked)
        {
            likes--;
            liked = false;
        }
        else
        {
            likes++;
            liked = true;
        }

        UpdateText();
    }

    void UpdateText()
    {
        if (targetTMP != null)
        {
            targetTMP.text = likes.ToString();
        }
    }
}