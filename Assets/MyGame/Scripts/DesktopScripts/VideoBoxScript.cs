using TMPro;
using UnityEngine;

public class VideoBoxScript : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] string targetWord = "apple";
    [SerializeField] TMP_Text sendComment;
    [SerializeField] int currentIndex = 0;
    [SerializeField] GameObject[] commentsTill;
    [SerializeField] GameObject[] commentsAfter;

    void Start()
    {
        inputField.text = "";
    }

    public void OpenComments()
    {
        foreach(GameObject com in commentsTill)
        {
            com.SetActive(true);
        }
    }

    public void ActivateCommentsAfter()
    {
        foreach(GameObject com in commentsAfter)
        {
            com.SetActive(true);
        }
    }
    public void OnInputChanged(string userInput)
    {
        if (currentIndex >= targetWord.Length)
        {
            inputField.interactable = false;
            return;
        }

        if (userInput.Length > currentIndex)
        {
            currentIndex++;

            string newText = targetWord.Substring(0, currentIndex);

            inputField.SetTextWithoutNotify(newText);

            if (currentIndex >= targetWord.Length)
            {
                inputField.gameObject.SetActive(false);
                sendComment.gameObject.SetActive(true);
                sendComment.text = targetWord;
                ActivateCommentsAfter();
            }
                
        }
    }
}