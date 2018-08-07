using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine.Networking;

public class AccountManagerScript : MonoBehaviour
{
    #region UI variables

    // Login panel objs
    public InputField inputUsername;
    public InputField inputPassword;

    // Status texts
    public Text statusMessage;
    public Text statusMessageNewAccount;

    // New account panel objs
    public GameObject pnlNewAccount;
    public InputField inputNewUsername;
    public InputField inputNewPassword;

    #endregion

    // This dictionary stores all login info and should be updating to server
    private Dictionary<string, string> loginInfo = new Dictionary<string, string>();

    // Keep this object to load other scenes with correct account info
    private void Awake()
    {
        DontDestroyOnLoad(transform);
    }

    // Use this for initialization
    void Start()
    {
        // Default inputs
        statusMessage.text = "";
        pnlNewAccount.SetActive(false);
        inputUsername.text = "";
        inputPassword.text = "";
    }

    private void Update()
    {
        // Enable tabbing to move focus from username to password
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputUsername.isFocused)
            {
                inputPassword.Select();
            }
            if (inputNewUsername.isFocused)
            {
                inputNewPassword.Select();
            }
        }
    }

    #region OnGUIs

    // Show new account panel
    public void OnBtnNewAcccountPressed()
    {
        // Show panel and set inputs empty
        inputUsername.text = "";
        inputPassword.text = "";
        pnlNewAccount.SetActive(true);
    }

    // Attempt to create new account
    public void OnBtnCreateAccountPressed()
    {
        bool isAbleToCreateNewAccount = false;

        // Check if username isn't blank
        if (inputNewUsername.text.Equals(""))
        {
            statusMessageNewAccount.text = "Enter your new username!";
            isAbleToCreateNewAccount = false;
        }
        // Check if password isn't blank
        else if (inputNewPassword.text.Equals(""))
        {
            statusMessageNewAccount.text = "Enter your new password";
            isAbleToCreateNewAccount = false;
        }
        // Check if username is already taken
        else if (loginInfo.ContainsKey(inputNewUsername.text))
        {
            statusMessageNewAccount.text = "That username is already taken.";
            isAbleToCreateNewAccount = false;
        }
        else
        {
            isAbleToCreateNewAccount = true;
        }

        // If login info is valid add new account
        if (isAbleToCreateNewAccount)
        {
            CreateNewAccount(inputNewUsername.text, inputNewPassword.text);
            statusMessageNewAccount.text = "Account created!";
        }

        Debug.Log(statusMessageNewAccount.text);
    }

    // Hide new account panel
    public void OnCloseNewAccountPanel()
    {
        pnlNewAccount.SetActive(false);
    }

    // If login info is valid, log in and open lobby scene
    public void OnPlayButtonPressed()
    {
        bool isAbleToPlay = false;
        string thisUsername = inputUsername.text;
        string thisPassword = inputPassword.text;

        // Check if username is blank
        if (thisUsername.Equals("") || thisUsername.Equals(null))
        {
            statusMessage.text = "Enter your username!";
            isAbleToPlay = false;
        }
        // Check if password is blank
        else if (thisPassword.Equals("") || thisPassword.Equals(null))
        {
            statusMessage.text = "Enter your password!";
            isAbleToPlay = false;

        }
        // Check if username and its corresponding password exists
        else if (loginInfo.TryGetValue(thisUsername, out thisPassword)) // Verify account
        {
            // Account exists
            isAbleToPlay = true;
        }
        // Incorrect username/password
        else
        {
            statusMessage.text = "Account not found";
            isAbleToPlay = false;
        }

        // Load lobby scene
        if (isAbleToPlay) 
        {
            statusMessage.text = "Loading...";
            SceneManager.LoadScene("LobbyScene");
        }

        Debug.Log(statusMessage.text);
    }

    #endregion 

    // Create new account with given params
    private void CreateNewAccount(string username, string password)
    {
        loginInfo.Add(username, password);
    }


}
