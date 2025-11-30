using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public TMP_InputField inputField;      
    public GameObject chatBubblePrefab;    
    public Transform chatContainer;        

    public void Senden()
    {
       
        if (string.IsNullOrWhiteSpace(inputField.text)) return;  
        GameObject bubble = Instantiate(chatBubblePrefab, chatContainer);
        TMP_Text[] textComponents = bubble.GetComponentsInChildren<TMP_Text>();

        if (textComponents.Length >= 2)
        {
            textComponents[1].text = inputField.text;
        }
        else
        {
            Debug.LogWarning("Es gibt weniger als 2 TMP_Text-Komponenten im Prefab.");
        }
        
        inputField.text = "";


        ScrollRect scrollRect = chatContainer.GetComponentInParent<ScrollRect>();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
