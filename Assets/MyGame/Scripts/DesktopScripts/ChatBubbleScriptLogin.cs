using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ChatBubbleScriptLogin : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField checkPasswordInput;
    [SerializeField] int loadingTime;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject loginScreen;


    [SerializeField] private GameObject registerButton;

    private void Awake()
    {
        StartCoroutine(WaitForLoading());
        nameInput.text = LoginScript.PlayerName;
        nameInput.ForceLabelUpdate();

        passwordInput.contentType = TMP_InputField.ContentType.Password;
        passwordInput.characterLimit = 1000;
        passwordInput.ForceLabelUpdate();

        checkPasswordInput.contentType = TMP_InputField.ContentType.Password;
        checkPasswordInput.characterLimit = 1000;
        checkPasswordInput.ForceLabelUpdate();

        registerButton.SetActive(false);

        nameInput.onValueChanged.AddListener(delegate { CheckFields(); });
        passwordInput.onValueChanged.AddListener(delegate { CheckFields(); });
        checkPasswordInput.onValueChanged.AddListener(delegate { CheckFields(); });

        CheckFields();
    }

    public IEnumerator WaitForLoading()
    {
        yield return new WaitForSeconds(loadingTime);
        loadingScreen.SetActive(false);
        loginScreen.SetActive(true);
    }
    private void CheckFields()
    {
        bool allValid =
            !string.IsNullOrEmpty(nameInput.text) &&
            passwordInput.text.Length >= 1 &&
            checkPasswordInput.text.Length >= 1;

        registerButton.SetActive(allValid);
    }
}