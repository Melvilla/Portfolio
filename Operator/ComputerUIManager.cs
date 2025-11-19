using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework;

public class ComputerUIManager : MonoBehaviour
{
    // UI panels for login and email screens
    [Header("Screens")]
    [SerializeField] GameObject LoginScreen;
    [SerializeField] GameObject EmailScreen;
    [SerializeField] List<GameObject> TabScreens;

    // Tab navigation buttons
    [Header("Tab UI")]
    [SerializeField] List<Button> TabButtons;

    // Login UI elements
    [Header("Login UI")]
    [SerializeField] TMP_InputField UsernameInput;
    [SerializeField] TMP_InputField PasswordInput;
    [SerializeField] Button LoginButton;
    [SerializeField] TMP_Text ErrorText;

    // Set your credentials here
    [SerializeField] string CorrectUsername;
    [SerializeField] string CorrectPassword;

    // Initialize UI and set up button listeners
    void Start()
    {
        LoginButton.onClick.AddListener(OnLoginClicked);
        ShowLoginScreen();

        // Assign tab switching to each tab button
        for (int i = 0; i < TabButtons.Count; i++)
        {
            int tabIndex = i;
            TabButtons[i].onClick.AddListener(() => ShowTab(tabIndex));
        }
    }

    // Handle login button click
    private void OnLoginClicked()
    {
        if (UsernameInput.text == CorrectUsername && PasswordInput.text == CorrectPassword)
        {
            ShowEmailScreen();
        }
        else
        {
            UsernameInput.text = "";
            PasswordInput.text = "";
            ErrorText.text = "Invalid username or password  please try again";
            StartCoroutine(HideErrorTextAfterDelay(5f));
        }
    }

    // Show the selected tab window, hide others
    private void ShowTab(int tabIndex)
    {
        for (int i = 0; i < TabScreens.Count; i++)
        {
            TabScreens[i].SetActive(i == tabIndex);
        }
    }

    // Hide error text after a delay
    private IEnumerator HideErrorTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ErrorText.text = "";
    }

    // Display login screen and hide email screen
    private void ShowLoginScreen()
    {
        LoginScreen.SetActive(true);
        EmailScreen.SetActive(false);
        ErrorText.text = "";
    }

    // Display email screen and hide login screen
    private void ShowEmailScreen()
    {
        LoginScreen.SetActive(false);
        EmailScreen.SetActive(true);
    }
}
