using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LoginScript : MonoBehaviour
{
    [SerializeField] private GameObject startPcParent;
    [SerializeField] private GameObject loginParent;
    [SerializeField] private GameObject pcLoginProcessParent;

    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private DesktopManager desktopManager;

    [SerializeField] private static string playerName;
    [SerializeField] public static string PlayerName => playerName = "Test Name";

    private void Awake()
    {
        passwordInput.characterLimit = 8;
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        passwordInput.ForceLabelUpdate();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            OpenLogin();
    }

    private void OpenLogin()
    {
        startPcParent.SetActive(false);
        loginParent.SetActive(true);
    }

    public void TryLogin()
    {
        if (string.IsNullOrEmpty(nameInput.text))
            return;

        if (passwordInput.text.Length != 8)
            return;

        playerName = nameInput.text;

        desktopManager.LoginFinished();
        pcLoginProcessParent.SetActive(false);
    }
}
