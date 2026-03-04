using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    [SerializeField] private GameObject startPcParent;
    [SerializeField] private GameObject loginParent;
    [SerializeField] private GameObject pcLoginProcessParent;

    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button loginButton;

    [SerializeField] private DesktopManager desktopManager;

    private static string playerName = "Testname";
    public static string PlayerName;

    private void Awake()
    {
        loginButton.gameObject.SetActive(false);
        nameInput.onValueChanged.AddListener(OnNameChanged);
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

    private void OnNameChanged(string value)
    {
        loginButton.gameObject.SetActive(!string.IsNullOrEmpty(value));
    }

    public void TryLogin()
    {
        if (string.IsNullOrEmpty(nameInput.text))
            return;

        playerName = nameInput.text;
        PlayerName = playerName;

        desktopManager.LoginFinished();
        pcLoginProcessParent.SetActive(false);
        gameObject.SetActive(false);
    }
}